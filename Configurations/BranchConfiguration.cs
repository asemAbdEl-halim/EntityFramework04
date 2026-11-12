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
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> entity)
        {
            entity.HasKey(b => b.BranchId);
            entity.Property(b => b.Code).HasMaxLength(20).IsRequired();
            entity.HasIndex(b => b.Code).IsUnique();
            entity.Property(b => b.Name).HasMaxLength(100).IsRequired();
            entity.Property(b => b.Address).HasMaxLength(250).IsRequired();
            entity.Property(b => b.PhoneNumber).HasMaxLength(20).IsRequired();

            entity.HasData(
                new Branch { BranchId = 1, Code = "CAI-01", Name = "Cairo Main Branch", Address = "Cairo, Egypt", PhoneNumber = "0223456789" },
                new Branch { BranchId = 2, Code = "ALX-01", Name = "Alexandria Branch", Address = "Alexandria, Egypt", PhoneNumber = "0334567890" }
            );
        }
    }

}
