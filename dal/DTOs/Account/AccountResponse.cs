namespace BankApi.dal.DTOs.Account;

public class AccountResponse
{
    public int Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Type { get; set; } = string.Empty;   
    public string Status { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}