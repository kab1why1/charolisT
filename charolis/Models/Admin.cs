using System;

namespace charolis.Models
{
    public class Admin : User
    {
        public string Role { get; set; } = "admin";

        public Admin() : base() { }

        public Admin(string name, string email, string passwordHash, string phone, string address)
            : base(name, email, passwordHash, phone, address)
        { }

        public override void ShowInfo()
        {
            Console.Write($"ID - {Id} Admin {Name}, Email: {Email}, PasswordHash: {PasswordHash}, Phone: {Phone}, Address: {Address}");
        }
    }

    public class AdminRepository : Repository<Admin> { }
}
