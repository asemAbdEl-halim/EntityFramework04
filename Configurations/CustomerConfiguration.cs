using EntityFramework04.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework04.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.HasKey(c => c.CustomerId);
            entity.Property(c => c.FullName).HasMaxLength(150).IsRequired();
            entity.Property(c => c.NationalId).HasMaxLength(20).IsRequired();
            entity.HasIndex(c => c.NationalId).IsUnique();
            entity.Property(c => c.Email).HasMaxLength(150).IsRequired();
            entity.HasIndex(c => c.Email).IsUnique();
            entity.Property(c => c.PhoneNumber).HasMaxLength(20).IsRequired();
            entity.Property(c => c.Address).HasMaxLength(250).IsRequired();
            entity.Property(c => c.CustomerType).HasConversion<string>().HasMaxLength(20);
        }
    }
}
