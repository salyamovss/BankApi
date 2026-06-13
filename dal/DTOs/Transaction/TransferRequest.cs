using System.ComponentModel.DataAnnotations;

namespace BankApi.dal.DTOs.Transaction;

public class TransferRequest
{
    [Required]
    public int FromAccountId { get; set; }

    [Required]
    public int ToAccountId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Сумма перевода должна быть больше нуля")]
    public decimal Amount { get; set; }

    public string? Description { get; set; }
}