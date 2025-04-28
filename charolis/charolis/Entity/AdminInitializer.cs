using charolis.DAL;
using charolis.Entity;
using System;
using System.Linq;
using charolis.Data;
using charolis.Services.Interfaces;

namespace charolis.Entities
{
    public static class AdminInitializer
    {
        public static void Initialize(IUserService userService)
        {
            // Якщо немає жодного адміна — створюємо
            if (!userService.GetAll().Any(u => u.Role == "Admin"))
            {
                var admin = new User
                {
                    Username     = "admin",
                    Password     = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role         = "Admin",
                    Email        = "admin@example.com",
                    Phone        = "1234567890",
                    Address      = "Адмінська адреса"
                };

                userService.Create(admin);
            }
        }
    }
}