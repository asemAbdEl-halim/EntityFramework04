using EntityFramework04.Configurations;
using EntityFramework04.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework04
{
    public class NationalBankContext : DbContext
    {
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<Manager> Managers => Set<Manager>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<CustomerAccount> CustomerAccounts => Set<CustomerAccount>();
        public DbSet<BankTransaction> Transactions => Set<BankTransaction>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.\\SQLEXPRESS;Database=NationalBankManagement;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BranchConfiguration());
            modelBuilder.ApplyConfiguration(new ManagerConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerAccountConfiguration());
            modelBuilder.ApplyConfiguration(new BankTransactionConfiguration());
        }
    }
}
