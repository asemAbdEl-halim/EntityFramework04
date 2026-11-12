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
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> entity)
        {
            entity.HasKey(m => m.ManagerId);
            entity.Property(m => m.FullName).HasMaxLength(150).IsRequired();
            entity.Property(m => m.Email).HasMaxLength(150).IsRequired();
            entity.Property(m => m.PhoneNumber).HasMaxLength(20).IsRequired();
            entity.HasIndex(m => m.Email).IsUnique();

            entity.HasOne(m => m.Branch)
                .WithOne(b => b.Manager)
                .HasForeignKey<Manager>(m => m.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasData(
                new Manager { ManagerId = 1, FullName = "Ahmed Hassan", Email = "ahmed.manager@bank.com", PhoneNumber = "01011112222", HireDate = new DateTime(2020, 1, 15), BranchId = 1 },
                new Manager { ManagerId = 2, FullName = "Mona Ali", Email = "mona.manager@bank.com", PhoneNumber = "01033334444", HireDate = new DateTime(2021, 3, 10), BranchId = 2 }
            );
        }
    }

}
