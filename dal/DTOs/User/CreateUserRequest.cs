using System.ComponentModel.DataAnnotations;
using BankApi.dal.Models.Enums;

namespace BankApi.dal.DTOs.User;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Имя обязательно для заполнения")]
    [StringLength(100, ErrorMessage = "Имя не должно превышать 100 символов")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Фамилия обязательна для заполнения")]
    [StringLength(100, ErrorMessage = "Фамилия не должна превышать 100 символов")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Отчество не должно превышать 100 символов")]
    public string MiddleName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    [StringLength(255, ErrorMessage = "Email не должен превышать 255 символов")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пол обязателен для заполнения")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Дата рождения обязательна для заполнения")]
    public DateOnly BirthDate { get; set; }

    [Required(ErrorMessage = "Серия паспорта обязательна для заполнения")]
    [StringLength(10, ErrorMessage = "Серия паспорта не должна превышать 10 символов")] // Например: AN или ID
    public string PassportSeries { get; set; } = string.Empty;

    [Required(ErrorMessage = "Номер паспорта обязателен для заполнения")]
    [StringLength(20, ErrorMessage = "Номер паспорта не должен превышать 20 символов")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Номер паспорта должен состоять только из цифр")]
    public string PassportNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Поле 'Кем выдан паспорт' обязательно для заполнения")]
    [StringLength(255, ErrorMessage = "Название органа выдачи не должно превышать 255 символов")]
    public string PassportIssuedBy { get; set; } = string.Empty;

    [Required(ErrorMessage = "Дата выдачи паспорта обязательна для заполнения")]
    public DateOnly PassportIssuedAt { get; set; }

    [Required(ErrorMessage = "Номер телефона обязателен для заполнения")]
    [Phone(ErrorMessage = "Некорректный формат номера телефона")]
    [StringLength(20, ErrorMessage = "Номер телефона не должен превышать 20 символов")]
    [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Номер телефона должен быть в международном формате (например, +996...)")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Тип телефона обязателен для выбора")]
    public PhoneType PhoneType { get; set; }
}