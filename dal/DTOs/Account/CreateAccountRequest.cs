using System.ComponentModel.DataAnnotations;
using BankApi.dal.Models.Enums;

namespace BankApi.dal.DTOs.Account;

public class CreateAccountRequest
{
    [Required(ErrorMessage = "Валюта обязательна для заполнения")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Код валюты должен состоять из 3 символов (например, KGS, USD)")]
    public string Currency { get; set; } = "KGS";

    [Required(ErrorMessage = "Тип счета обязателен")]
    public AccountType Type { get; set; } 
}