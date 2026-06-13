using System.ComponentModel.DataAnnotations;
using BankApi.dal.Models.Enums;

namespace BankApi.dal.DTOs.User;

public class UpdateUserRequest
{
    [Required(ErrorMessage = "Имя обязательно для заполнения")]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Фамилия обязательна для заполнения")]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(100)]
    public string MiddleName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public Gender Gender { get; set; }
}