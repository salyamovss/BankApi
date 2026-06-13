namespace BankApi.dal.DTOs.Phone;

public class PhoneResponse
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}