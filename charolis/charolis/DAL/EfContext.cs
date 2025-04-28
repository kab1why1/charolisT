using charolis.DAL.Configuration;
using charolis.Entity;
using Microsoft.EntityFrameworkCore;

namespace charolis.DAL;

public class EfContext : DbContext
{
    public EfContext(DbContextOptions<EfContext> options) 
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        
        //seed
        DbSeeder.Seed(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }
}