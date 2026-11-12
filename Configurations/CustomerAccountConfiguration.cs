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
    public class CustomerAccountConfiguration : IEntityTypeConfiguration<CustomerAccount>
    {
        public void Configure(EntityTypeBuilder<CustomerAccount> entity)
        {
            entity.HasKey(ca => new { ca.CustomerId, ca.AccountNumber });
            entity.Property(ca => ca.OwnershipType).HasConversion<string>().HasMaxLength(20);
            entity.Property(ca => ca.AccountStatus).HasConversion<string>().HasMaxLength(20);

            entity.HasOne(ca => ca.Customer)
                .WithMany(c => c.CustomerAccounts)
                .HasForeignKey(ca => ca.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ca => ca.Account)
                .WithMany(a => a.CustomerAccounts)
                .HasForeignKey(ca => ca.AccountNumber)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
