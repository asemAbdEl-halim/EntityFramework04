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
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> entity)
        {
            entity.HasKey(a => a.AccountNumber);
            entity.Property(a => a.AccountNumber).HasMaxLength(30);
            entity.Property(a => a.CurrentBalance).HasColumnType("decimal(18,2)");
            entity.Property(a => a.AccountType).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(a => a.Branch)
                .WithMany(b => b.Accounts)
                .HasForeignKey(a => a.BranchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
