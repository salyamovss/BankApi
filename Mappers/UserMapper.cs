using BankApi.Common;
using BankApi.dal.DTOs.Phone;
using BankApi.dal.DTOs.User;
using BankApi.dal.Models;

namespace BankApi.Mappers;

public class UserMapper
{
    public UserResponse ToResponse(User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        MiddleName = user.MiddleName,
        Email = user.Email,
        Gender = user.Gender.ToString(),
        BirthDate = user.BirthDate,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt,

        PassportSeries = user.Passport?.Series ?? string.Empty,
        PassportNumber = user.Passport != null
            ? BankHelper.MaskPassportNumber(user.Passport.Number)
            : string.Empty,

        Phones = user.Phones.Select(p => new PhoneResponse
        {
            Id = p.Id,
            Number = p.Number,
            Type = p.Type.ToString()
        }).ToList()
    };

    public User ToEntity(CreateUserRequest request)
    {
        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            MiddleName = (request.MiddleName ?? string.Empty).Trim(),
            Email = request.Email.Trim().ToLower(),
            Gender = request.Gender,
            BirthDate = request.BirthDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        user.Passport = new PassportDetail
        {
            Series = request.PassportSeries.ToUpper().Trim(),
            Number = request.PassportNumber.Trim(),
            IssuedBy = request.PassportIssuedBy.Trim(),
            IssuedAt = request.PassportIssuedAt
        };

        user.Phones.Add(new Phone
        {
            Number = request.PhoneNumber.Trim(),
            Type = request.PhoneType,
            CreatedAt = DateTime.UtcNow
        });

        return user;
    }

    public void UpdateEntity(UpdateUserRequest request, User existingUser)
    {
        existingUser.FirstName = request.FirstName.Trim();
        existingUser.LastName = request.LastName.Trim();
        existingUser.MiddleName = (request.MiddleName ?? string.Empty).Trim();
        existingUser.Email = request.Email.Trim().ToLower();
        existingUser.Gender = request.Gender;
    }
}