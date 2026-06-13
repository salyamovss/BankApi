using System.ComponentModel.DataAnnotations;
using BankApi.dal.Models.Enums;

namespace BankApi.dal.DTOs.Phone;

public class AddPhoneRequest
{
    [Required]
    [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Номер телефона должен быть в международном формате")]
    [StringLength(20)]
    public string Number { get; set; } = string.Empty;

    [Required]
    public PhoneType Type { get; set; }
}