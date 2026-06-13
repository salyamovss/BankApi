using System.ComponentModel.DataAnnotations;

namespace BankApi.dal.DTOs.User;

public class RestoreUserRequest
{
    [Required(ErrorMessage = "Email обязателен для восстановления")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    public string Email { get; set; } = string.Empty;
}