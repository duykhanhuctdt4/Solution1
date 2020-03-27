using DuyKhanhSolution1.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DuyKhanhSolution1.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Price).IsRequired();
            builder.Property(t => t.Stock).IsRequired().HasDefaultValue(0);
            builder.Property(t => t.ViewCount).IsRequired().HasDefaultValue(0);
        }
    }
}
