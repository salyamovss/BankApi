using BankApi.dal.Models.Enums;

namespace BankApi.dal.Models;

public class Phone
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public PhoneType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}