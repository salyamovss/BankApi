using System.Net;
using BankApi.Common;
using BankApi.dal.DTOs.User;
using BankApi.dal.Models.Enums;
using BankApi.dal.Repositories;
using BankApi.Data;
using BankApi.Mappers;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Services;

public class UserService(IUserRepository userRepository, AppDbContext db, UserMapper mapper)
{
 public async Task<UserResponse> Create(CreateUserRequest request)
{
    string cleanEmail = request.Email.Trim().ToLower();

    var existingUser = await userRepository.GetByEmailWithDetailsAsync(cleanEmail);
    if (existingUser != null)
    {
        if (existingUser.IsActive)
            throw new AppException(ErrorCodes.UserAlreadyExists, HttpStatusCode.Conflict);

        throw new AppException(ErrorCodes.UserDeactivatedUseRestoreEndpoint, HttpStatusCode.Conflict);
    }

    var newUser = mapper.ToEntity(request);
    await db.Users.AddAsync(newUser);
    await db.SaveChangesAsync();

    return mapper.ToResponse(newUser);
}

public async Task<UserResponse> Restore(RestoreUserRequest request)
{
    string cleanEmail = request.Email.Trim().ToLower();

    var user = await userRepository.GetByEmailWithDetailsAsync(cleanEmail)
        ?? throw new AppException(ErrorCodes.UserNotFound, HttpStatusCode.NotFound);

    if (user.IsActive)
        throw new AppException(ErrorCodes.UserAlreadyActive, HttpStatusCode.Conflict);

    user.IsActive = true;
    await db.SaveChangesAsync();

    return mapper.ToResponse(user);
}

public async Task<DeactivateResponse> Deactivate(int id)
{
        var user = await db.Users
                       .Include(u => u.Accounts)
                       .ThenInclude(a => a.Cards)
                       .FirstOrDefaultAsync(u => u.Id == id)
                   ?? throw new AppException(ErrorCodes.UserNotFound, HttpStatusCode.NotFound);

        if (!user.IsActive)
            throw new AppException(ErrorCodes.UserAlreadyDeactivated, HttpStatusCode.Conflict);

        var closedAccounts = new List<string>();

        foreach (var account in user.Accounts.Where(a => a.Status == AccountStatus.Active))
        {
            if (account.Balance == 0)
            {
                foreach (var card in account.Cards)
                    card.Status = CardStatus.Blocked;

                account.Status = AccountStatus.Closed;
                account.ClosedAt = DateTime.UtcNow;
                closedAccounts.Add(account.AccountNumber);
            }
            else
            {
                foreach (var card in account.Cards.Where(c => c.Status == CardStatus.Active))
                    card.Status = CardStatus.Blocked;
            }
        }

        user.IsActive = false;
        await db.SaveChangesAsync();

        return new DeactivateResponse
        {
            Message = ErrorMessages.Get(ErrorCodes.UserDeactivatedSuccess),
            ClosedAccounts = closedAccounts
        };
    }

    public async Task<UserResponse> GetById(int id)
    {
        var user = await userRepository.GetByIdWithDetailsAsync(id)
            ?? throw new AppException(ErrorCodes.UserNotFound, HttpStatusCode.NotFound);

        return mapper.ToResponse(user);
    }

    public async Task<UserResponse> UpdateAsync(int id, UpdateUserRequest request)
    {
        string cleanEmail = request.Email.Trim().ToLower();

        var user = await userRepository.GetByIdWithDetailsAsync(id)
            ?? throw new AppException(ErrorCodes.UserNotFound, HttpStatusCode.NotFound);

        if (user.Email != cleanEmail)
        {
            var userWithSameEmail = await userRepository.GetByEmailWithDetailsAsync(cleanEmail);
            if (userWithSameEmail != null && userWithSameEmail.Id != id)
                throw new AppException(ErrorCodes.UserAlreadyExists, HttpStatusCode.Conflict);
        }

        mapper.UpdateEntity(request, user);
        await db.SaveChangesAsync();

        return mapper.ToResponse(user);
    }

    public async Task<List<UserResponse>> GetFilteredAsync(UserFilterRequest filter)
    {
        var users = await userRepository.GetFilteredAsync(filter);
        return users.Select(mapper.ToResponse).ToList();
    }
}