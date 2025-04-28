using System.ComponentModel.DataAnnotations;

namespace charolis.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Логін обов'язковий")]
    [Display(Name = "Логін")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Пароль обов'язковий")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }
}