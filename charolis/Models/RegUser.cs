using System;

namespace charolis.Models;

public class RegUser : User
{
    public string Role { get; set; } = "Registered User";

    public RegUser() : base() { }

    public RegUser(string name, string email, string password, string phone, string address) 
        : base(name, email, password, phone, address)
    { }

    public override void ShowInfo()
    {
        Console.Write($"ID - {Id}, Registered User {Name}. Email: {Email}, Password: {PasswordHash}, Phone: {Phone}, Address: {Address}");
    }

}

public class RegUserRepository : Repository<RegUser> { }
