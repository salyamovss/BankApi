namespace BankApi.dal.Models;

public class PassportDetail
{
    public int Id { get; set; }
    public string Series { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string IssuedBy { get; set; } = string.Empty;
    public DateOnly IssuedAt { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}