using System.ComponentModel.DataAnnotations;

namespace charolis.Models.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Логін обов'язковий")]
    [Display(Name = "Логін")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email обов'язковий")]
    [EmailAddress(ErrorMessage = "Невірний формат email")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Телефон обов'язковий")]
    [Phone(ErrorMessage = "Невірний формат телефону")]
    [Display(Name = "Телефон")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Адреса обов'язкова")]
    [Display(Name = "Адреса")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Пароль обов'язковий")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Підтвердження пароля обов'язкове")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Паролі не співпадають")]
    [Display(Name = "Підтвердіть пароль")]
    public string ConfirmPassword { get; set; }
}