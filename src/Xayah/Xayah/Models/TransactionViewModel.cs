using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xayah.Models
{
    public class TransactionViewModel
    {
        public Guid Id { get; set; }
        public string BankId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string Type { get; set; }
        public DateTime DatePosted { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; }
        public DateTime DateImport { get; set; }
    }
}
