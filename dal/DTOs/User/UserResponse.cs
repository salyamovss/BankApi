using BankApi.dal.DTOs.Phone;

namespace BankApi.dal.DTOs.User;

public class UserResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public string PassportSeries { get; set; } = string.Empty;
    public string PassportNumber { get; set; } = string.Empty;

    public List<PhoneResponse> Phones { get; set; } = [];
}