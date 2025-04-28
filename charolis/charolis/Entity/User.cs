using System.ComponentModel.DataAnnotations;

namespace charolis.Entity;

public abstract class BaseEntity
{
    public abstract int Id { get; set; }
    public void GetInfo() => Id = 10;
    public virtual void SetInfo(int newId) => Id = newId;
}

public class User : BaseEntity
{
    public override int Id { get; set; }

    [Required(ErrorMessage = "Логін обов'язковий")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Логін має бути від 3 до 50 символів")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Пароль обов'язковий")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має бути від 6 до 100 символів")]
    public string Password { get; set; }
    [Required]
    public string Role { get; set; } = "User";
    [Required(ErrorMessage = "Email обов'язковий")]
    [EmailAddress(ErrorMessage = "Некоректний формат Email")]
    public string? Email { get; set; }
    [Phone(ErrorMessage = "Некоректний формат телефону")]
    public string? Phone { get; set; }
    [Required(ErrorMessage = "Адреса обов'язкова")]
    [StringLength(200, ErrorMessage = "Адреса не може перевищувати 200 символів")]
    public string? Address { get; set; }
    public override void SetInfo(int newId)
    {
        base.SetInfo(newId);
        Console.WriteLine(newId);   
    }
}

public class Admin : User
{
    public string AdminUsername { get; private set; }
    public string AdminPassword { get; private set; }
}