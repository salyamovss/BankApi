using BankApi.dal.DTOs.Phone;
using BankApi.dal.Models;

namespace BankApi.Mappers;

public class PhoneMapper
{
    public PhoneResponse ToResponse(Phone phone) => new()
    {
        Id = phone.Id,
        Number = phone.Number,
        Type = phone.Type.ToString()
    };

    public Phone ToEntity(AddPhoneRequest request, int userId) => new()
    {
        Number = request.Number.Trim(),
        Type = request.Type,
        UserId = userId,
        CreatedAt = DateTime.UtcNow
    };
}