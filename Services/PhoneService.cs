using System.Net;
using BankApi.Common;
using BankApi.dal.DTOs.Phone;
using BankApi.dal.Repositories;
using BankApi.Mappers;

namespace BankApi.Services;

public class PhoneService(IPhoneRepository phoneRepository, PhoneMapper mapper)
{
    public async Task<PhoneResponse> Add(int userId, AddPhoneRequest request)
    {
        if (!await phoneRepository.UserExistsAsync(userId))
            throw new AppException(ErrorCodes.UserNotFound, HttpStatusCode.NotFound);

        string cleanNumber = request.Number.Trim();

        if (await phoneRepository.IsNumberTakenAsync(cleanNumber))
            throw new AppException(ErrorCodes.PhoneAlreadyExists, HttpStatusCode.Conflict);

        var phone = mapper.ToEntity(request, userId);
        await phoneRepository.AddAsync(phone);
        await phoneRepository.SaveChangesAsync();

        return mapper.ToResponse(phone);
    }

    public async Task Delete(int phoneId, int userId)
    {
        var phone = await phoneRepository.GetByIdAsync(phoneId)
                    ?? throw new AppException(ErrorCodes.PhoneNotFound, HttpStatusCode.NotFound);

        if (phone.UserId != userId)
            throw new AppException(ErrorCodes.PhoneNotBelongToUser, HttpStatusCode.Forbidden);

        int totalPhonesCount = await phoneRepository.CountByUserIdAsync(userId);
        if (totalPhonesCount <= 1)
            throw new AppException(ErrorCodes.CannotDeleteLastPhoneNumber, HttpStatusCode.BadRequest);

        await phoneRepository.RemoveAsync(phone);
        await phoneRepository.SaveChangesAsync();
    }
}