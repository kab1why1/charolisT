using System;

namespace charolis.Models;

public class Admin : User
{
    public string Role { get; set; } = "admin";

    public Admin() : base() { }

    public Admin(string name, string email, string password, string phone, string address) 
        : base(name, email, password, phone, address)
    { }

    public override void ShowInfo() {
        Console.Write($"ID - {Id} Admin {Name}, Email: {Email}, Password: {Password}, Phone: {Phone}, Address: {Address}");
    }

    
}

public class AdminRepository : Repository<Admin> { }
