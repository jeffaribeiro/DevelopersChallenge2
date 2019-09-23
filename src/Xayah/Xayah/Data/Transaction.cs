using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xayah.Data
{
    public class Transaction
    { 
        public Guid Id { get; private set; }
        public string BankId { get; private set; }
        public string AccountNumber { get; private set; }
        public string AccountType { get; private set; }
        public string Type { get; private set; }
        public DateTime DatePosted { get; private set; }
        public decimal Amount { get; private set; }
        public string Memo { get; private set; }
        public DateTime DateImport { get; private set; }

        protected Transaction() { }

        public Transaction(string bankId, string accountNumber, string accountType, string type, DateTime datePosted, decimal amount, string memo)
        {
            Id = Guid.NewGuid();
            BankId = bankId;
            AccountNumber = accountNumber;
            AccountType = accountType;
            Type = type;
            DatePosted = datePosted;
            Amount = amount;
            Memo = memo;
            DateImport = DateTime.Now;
        }
    }
}
