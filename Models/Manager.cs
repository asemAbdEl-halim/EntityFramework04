using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework04.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
    }
}
