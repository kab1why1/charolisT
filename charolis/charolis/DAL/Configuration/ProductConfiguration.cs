using charolis.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace charolis.DAL.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(p => p.Description)
            .HasMaxLength(200);
        
        builder.Property(p => p.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();
        
        builder.Property(p => p.IsActive)
            .IsRequired();
        
        builder.ToTable("Products");
    }
}