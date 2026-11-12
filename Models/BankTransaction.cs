using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework04.Models
{
    public class BankTransaction
    {
        public int TransactionId { get; set; }
        public string TransactionNumber { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? Note { get; set; }

        public string AccountNumber { get; set; } = string.Empty;
        public Account Account { get; set; } = null!;
    }

}
