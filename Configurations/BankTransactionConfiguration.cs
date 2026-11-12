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
    public class BankTransactionConfiguration : IEntityTypeConfiguration<BankTransaction>
    {
        public void Configure(EntityTypeBuilder<BankTransaction> entity)
        {
            entity.HasKey(t => t.TransactionId);
            entity.Property(t => t.TransactionNumber).HasMaxLength(30).IsRequired();
            entity.HasIndex(t => t.TransactionNumber).IsUnique();
            entity.Property(t => t.Amount).HasColumnType("decimal(18,2)");
            entity.Property(t => t.TransactionType).HasConversion<string>().HasMaxLength(20);
            entity.Property(t => t.Note).HasMaxLength(500);

            entity.HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountNumber)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
