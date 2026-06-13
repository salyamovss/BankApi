using System.Net;
using BankApi.Common;
using BankApi.dal.DTOs.Phone;
using BankApi.dal.Repositories;
using BankApi.Data;
using BankApi.Mappers;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Services;

public class PhoneService(AppDbContext db, IPhoneRepository phoneRepository, PhoneMapper mapper)
{
    public async Task<PhoneResponse> Add(int userId, AddPhoneRequest request)
    {
        if (!await db.Users.AnyAsync(u => u.Id == userId && u.IsActive))
            throw new AppException(ErrorCodes.UserNotFound, HttpStatusCode.NotFound);

        string cleanNumber = request.Number.Trim();

        bool isPhoneTaken = await db.Phones.AnyAsync(p => p.Number == cleanNumber);
        if (isPhoneTaken)
            throw new AppException(ErrorCodes.PhoneAlreadyExists, HttpStatusCode.Conflict);

        var phone = mapper.ToEntity(request, userId);
        db.Phones.Add(phone);
        await db.SaveChangesAsync();

        return mapper.ToResponse(phone);
    }

    public async Task Delete(int phoneId, int userId)
    {
        var phone = await phoneRepository.GetByIdAsync(phoneId)
                    ?? throw new AppException(ErrorCodes.PhoneNotFound, HttpStatusCode.NotFound);

        if (phone.UserId != userId)
            throw new AppException(ErrorCodes.PhoneNotBelongToUser, HttpStatusCode.Forbidden);

        int totalPhonesCount = await db.Phones.CountAsync(p => p.UserId == userId);
        if (totalPhonesCount <= 1)
        {
            throw new AppException(ErrorCodes.CannotDeleteLastPhoneNumber, HttpStatusCode.BadRequest);
        }

        db.Phones.Remove(phone);
        await db.SaveChangesAsync();
    }
}