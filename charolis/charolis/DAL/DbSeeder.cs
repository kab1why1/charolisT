using charolis.Entity;
using Microsoft.EntityFrameworkCore;

namespace charolis.DAL;

public class DbSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Users
        modelBuilder.Entity<User>().HasData(
            // new User { Id = 1, Username = "admin", PasswordHash = "admin123", Role = "Admin", Email = "admin@example.com", Phone = "1234567890", Address = "Admin Street 1" },
            new User { Id = 2, Username = "user1", Password = "password1", Role = "User", Email = "user1@example.com", Phone = "0987654321", Address = "User Road 5" }
        );

        // Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Magic Wand", Description = "Powerful wand", Price = 49.99m, IsActive = true },
            new Product { Id = 2, Name = "Potion of Invisibility", Description = "Makes you invisible for 5 mins", Price = 19.99m, IsActive = true }
        );

        // Orders
        modelBuilder.Entity<Order>().HasData(
            new Order { Id = 1, UserId = 2, Total = 89.97m, CreatedAt = DateTime.UtcNow }
        );

        // OrderItems
        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, PriceAtPurchase = 49.99m },
            new OrderItem { Id = 2, OrderId = 1, ProductId = 2, Quantity = 2, PriceAtPurchase = 19.99m }
        );
    }
}