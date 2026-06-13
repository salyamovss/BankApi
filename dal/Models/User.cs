using BankApi.dal.Models.Enums;

namespace BankApi.dal.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateOnly BirthDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public PassportDetail? Passport { get; set; }
    public ICollection<Phone> Phones { get; set; } = new List<Phone>();
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}