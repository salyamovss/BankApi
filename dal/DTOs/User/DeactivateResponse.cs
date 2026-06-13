namespace BankApi.dal.DTOs.User;

public class DeactivateResponse
{
    public string Message { get; set; } = string.Empty;
    public List<string> ClosedAccounts { get; set; } = new();
}