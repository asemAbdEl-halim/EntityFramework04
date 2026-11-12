using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework04.Models
{
    public enum CustomerType
    {
        Individual = 1,
        Business = 2
    }

    public enum AccountType
    {
        Savings = 1,
        Current = 2,
        Business = 3
    }

    public enum OwnershipType
    {
        Primary = 1,
        CoHolder = 2
    }

    public enum AccountStatus
    {
        Active = 1,
        Closed = 2
    }

    public enum TransactionType
    {
        Deposit = 1,
        Withdrawal = 2,
        Transfer = 3
    }

}
