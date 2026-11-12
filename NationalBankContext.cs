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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.ApplyConfigurationsFromAssembly(typeof(NationalBankContext).Assembly);
        //}
    }
}
