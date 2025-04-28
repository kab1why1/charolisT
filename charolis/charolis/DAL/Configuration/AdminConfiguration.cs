﻿using charolis.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace charolis.DAL.Configuration;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.Property(a => a.AdminUsername).IsRequired().HasMaxLength(50);
        builder.Property(a => a.AdminPassword).IsRequired();

        builder.ToTable("Admins");
    }
}