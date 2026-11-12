using EntityFramework04.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EntityFramework04
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var context = new NationalBankContext();

            // Migration----------------------------------------------------
            context.Database.Migrate();

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("       National Bank - Management");
                Console.WriteLine("========================================");
                Console.WriteLine("  1) Add a new Customer");
                Console.WriteLine("  2) Open a new Account for a Customer");
                Console.WriteLine("  3) Update Account Status (Active / Closed)");
                Console.WriteLine("  4) Remove an Account from a Customer");
                Console.WriteLine("  5) List all Customers (with accounts)");
                Console.WriteLine("  0) Exit");
                Console.WriteLine("----------------------------------------");

                int choice = ReadInt("Enter choice: ");

                try
                {
                    switch (choice)
                    {
                        case 1:
                            await AddCustomer(context);
                            break;
                        case 2:
                            await OpenAccount(context);
                            break;
                        case 3:
                            await UpdateAccountStatus(context);
                            break;
                        case 4:
                            await RemoveAccountFromCustomer(context);
                            break;
                        case 5:
                            await ListCustomers(context);
                            break;
                        case 0:
                            exit = true;
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please choose from the menu.");
                            break;
                    }
                }
                catch (DbUpdateException)
                {
                    Console.WriteLine("Database error. Check that the entered values are valid and not duplicated.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Something went wrong: {ex.Message}");
                }

                if (!exit)
                {
                    Console.WriteLine();
                    Console.WriteLine("Press any key to return to the menu...");
                    Console.ReadKey();
                }
            }
        }

        // Create Customer-----------------------------------------------
        static async Task AddCustomer(NationalBankContext context)
        {
            Console.WriteLine("--- Add New Customer ---");

            string fullName = ReadRequired("Full Name: ");
            string nationalId = ReadRequired("National ID: ");
            DateTime dateOfBirth = ReadDate("Date of Birth (yyyy-MM-dd): ");
            string email = ReadRequired("Email: ");
            string phone = ReadRequired("Phone: ");
            string address = ReadRequired("Address: ");

            Console.WriteLine("Customer Type:");
            Console.WriteLine("  1) Individual");
            Console.WriteLine("  2) Business");
            int typeChoice = ReadInt("Choice: ");

            if (typeChoice != 1 && typeChoice != 2)
            {
                Console.WriteLine("Invalid customer type.");
                return;
            }

            var customer = new Customer
            {
                FullName = fullName,
                NationalId = nationalId,
                DateOfBirth = dateOfBirth,
                Email = email,
                PhoneNumber = phone,
                Address = address,
                CustomerType = (CustomerType)typeChoice
            };

            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            Console.WriteLine($"Customer created successfully. CustomerId = {customer.CustomerId}");
        }

        // Open Account---------------------------------------------------
        static async Task OpenAccount(NationalBankContext context)
        {
            Console.WriteLine("--- Open New Account ---");

            string accountNumber = ReadRequired("Account Number: ");
            Console.WriteLine("Account Type:");
            Console.WriteLine("  1) Savings");
            Console.WriteLine("  2) Current");
            Console.WriteLine("  3) Business");
            int accountTypeChoice = ReadInt("Choice: ");

            if (accountTypeChoice < 1 || accountTypeChoice > 3)
            {
                Console.WriteLine("Invalid account type.");
                return;
            }

            string branchCode = ReadRequired("Branch Code: ");
            int customerId = ReadInt("Customer Id: ");

            Console.WriteLine("Ownership Role:");
            Console.WriteLine("  1) Primary");
            Console.WriteLine("  2) CoHolder");
            int ownershipChoice = ReadInt("Choice: ");

            if (ownershipChoice != 1 && ownershipChoice != 2)
            {
                Console.WriteLine("Invalid ownership role.");
                return;
            }

            var branch = await context.Branches.FirstOrDefaultAsync(b => b.Code == branchCode);
            var customer = await context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (branch == null)
            {
                Console.WriteLine("Branch does not exist.");
                return;
            }

            if (customer == null)
            {
                Console.WriteLine("Customer does not exist.");
                return;
            }

            bool accountExists = await context.Accounts.AnyAsync(a => a.AccountNumber == accountNumber);
            if (accountExists)
            {
                Console.WriteLine("This account number already exists.");
                return;
            }

            var account = new Account
            {
                AccountNumber = accountNumber,
                CurrentBalance = 0,
                AccountType = (AccountType)accountTypeChoice,
                OpeningDate = DateTime.Now,
                BranchId = branch.BranchId
            };

            var customerAccount = new CustomerAccount
            {
                CustomerId = customerId,
                AccountNumber = accountNumber,
                OwnershipType = (OwnershipType)ownershipChoice,
                OwnershipStartDate = DateTime.Now,
                AccountStatus = AccountStatus.Active
            };

            context.Accounts.Add(account);
            context.CustomerAccounts.Add(customerAccount);
            await context.SaveChangesAsync();

            Console.WriteLine($"Validating branch '{branch.Code}' and customer #{customerId}...");
            Console.WriteLine($"Account '{accountNumber}' created and linked to customer #{customerId} as {customerAccount.OwnershipType} owner.");
        }

        // Update Account Status-----------------------------------------
        static async Task UpdateAccountStatus(NationalBankContext context)
        {
            Console.WriteLine("--- Update Account Status ---");

            string accountNumber = ReadRequired("Account Number: ");
            int customerId = ReadInt("Customer Id: ");

            var customerAccount = await context.CustomerAccounts
                .FirstOrDefaultAsync(ca => ca.AccountNumber == accountNumber && ca.CustomerId == customerId);

            if (customerAccount == null)
            {
                Console.WriteLine("This customer does not own this account.");
                return;
            }

            customerAccount.AccountStatus = customerAccount.AccountStatus == AccountStatus.Active
                ? AccountStatus.Closed
                : AccountStatus.Active;

            await context.SaveChangesAsync();
            Console.WriteLine($"Status updated to {customerAccount.AccountStatus}.");
        }

        // Remove Account From Customer----------------------------------
        static async Task RemoveAccountFromCustomer(NationalBankContext context)
        {
            Console.WriteLine("--- Remove Account From Customer ---");

            string accountNumber = ReadRequired("Account Number: ");
            int customerId = ReadInt("Customer Id: ");

            var customerAccount = await context.CustomerAccounts
                .FirstOrDefaultAsync(ca => ca.AccountNumber == accountNumber && ca.CustomerId == customerId);

            if (customerAccount == null)
            {
                Console.WriteLine("Customer account link was not found.");
                return;
            }

            context.CustomerAccounts.Remove(customerAccount);
            await context.SaveChangesAsync();

            Console.WriteLine("Ownership link deleted.");

            bool hasOtherOwners = await context.CustomerAccounts.AnyAsync(ca => ca.AccountNumber == accountNumber);
            if (!hasOtherOwners)
            {
                var account = await context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
                if (account != null)
                {
                    context.Accounts.Remove(account);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"That was the last owner - account '{accountNumber}' was also removed.");
                }
            }
        }

        // List Customers With Accounts----------------------------------
        static async Task ListCustomers(NationalBankContext context)
        {
            Console.WriteLine("--- All Customers ---");

            var customers = await context.Customers
                .Include(c => c.CustomerAccounts)
                    .ThenInclude(ca => ca.Account)
                        .ThenInclude(a => a.Branch)
                .AsNoTracking()
                .ToListAsync();

            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
                return;
            }

            foreach (var customer in customers)
            {
                Console.WriteLine($"\n#{customer.CustomerId} {customer.FullName} ({customer.CustomerType})");

                if (customer.CustomerAccounts.Count == 0)
                {
                    Console.WriteLine("   (no accounts)");
                    continue;
                }

                foreach (var link in customer.CustomerAccounts)
                {
                    var account = link.Account;
                    Console.WriteLine(
                        $"   {account.AccountNumber,-12} {account.AccountType,-10} Balance: {account.CurrentBalance,10:N2}  {link.OwnershipType,-8} {link.AccountStatus,-7} @ {account.Branch.Name}");
                }
            }
        }

        // Input Helpers-------------------------------------------------
        static string ReadRequired(string message)
        {
            while (true)
            {
                Console.Write(message);
                string? value = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(value))
                    return value.Trim();

                Console.WriteLine("This field is required. Please try again.");
            }
        }

        static int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;

                Console.WriteLine("Please enter a valid number.");
            }
        }

        static DateTime ReadDate(string message)
        {
            while (true)
            {
                Console.Write(message);
                if (DateTime.TryParseExact(
                    Console.ReadLine(),
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime date))
                {
                    if (date.Date < DateTime.Today)
                        return date;

                    Console.WriteLine("Date of birth must be in the past.");
                }
                else
                {
                    Console.WriteLine("Please enter the date in yyyy-MM-dd format.");
                }
            }
        }
    }
}
