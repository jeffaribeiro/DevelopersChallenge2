using Microsoft.AspNetCore.Http;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xayah.Data;
using Xayah.Models;

namespace Xayah.Util
{
    public class OFXReader
    {
        private string BankId;
        private string AccountNumber;
        private string AccountType;
        private List<OFXTransaction> OFXTransactions;
        private string OFXStream;

        public OFXReader()
        {
            OFXTransactions = new List<OFXTransaction>();
        }

        public List<Transaction> ReadTransactionsFromFiles(IFormFile[] files)
        {
            List<Transaction> importedTransactions = new List<Transaction>();

            foreach (var file in files)
            {
                using (Stream stream = file.OpenReadStream())
                {
                    OFXStream = new StreamReader(stream, Encoding.Default).ReadToEnd();

                    this.BankId = this.GetTagValue("<BANKID>");
                    this.AccountNumber = this.GetTagValue("<ACCTID>");
                    this.AccountType = this.GetTagValue("<ACCTTYPE>");
                    this.FindTransactionsInFile();
                }
            }

            foreach (var item in this.OFXTransactions)
            {
                Transaction transaction = this.GenereteInput(item);
                importedTransactions.Add(transaction);
            }

            return importedTransactions.DistinctBy(t =>
            new { t.AccountNumber, t.AccountType, t.Amount, t.BankId, t.DatePosted, t.Memo, t.Type }).ToList();
        }

        #region "Private methods"

        private void FindTransactionsInFile()
        {

            int indexCursorTransactions = 0;
            int index = 0;

            while (index >= 0)
            {
                index = OFXStream.IndexOf("<STMTTRN>", indexCursorTransactions);

                if (index > 0)
                {
                    OFXTransaction transaction =
                        new OFXTransaction(this.GetTagValue("<TRNTYPE>", index),
                                           this.GetTagValue("<DTPOSTED>", index),
                                           this.GetTagValue("<TRNAMT>", index),
                                           this.GetTagValue("<MEMO>", index));

                    this.OFXTransactions.Add(transaction);
                    indexCursorTransactions = index + 1;
                }

            }
        }

        private Transaction GenereteInput(OFXTransaction ofxTransaction)
        {
            int year = int.Parse(ofxTransaction.DatePosted.Substring(0, 4));
            int month = int.Parse(ofxTransaction.DatePosted.Substring(4, 2));
            int day = int.Parse(ofxTransaction.DatePosted.Substring(6, 2));
            int hour = int.Parse(ofxTransaction.DatePosted.Substring(8, 2));
            int minute = int.Parse(ofxTransaction.DatePosted.Substring(10, 2));
            int second = int.Parse(ofxTransaction.DatePosted.Substring(12, 2));

            DateTime datePosted = new DateTime(year, month, day, hour, minute, second);
            decimal amount = decimal.Parse(ofxTransaction.Amount.Replace(".", ","));
            
            Transaction transaction = new Transaction(this.BankId, 
                                                      this.AccountNumber,
                                                      this.AccountType,
                                                      ofxTransaction.Type,
                                                      datePosted,
                                                      amount,
                                                      ofxTransaction.Memo);

            return transaction;
        }

        private string GetTagValue(string tag, int index)
        {
            string value = null;

            int ind = OFXStream.IndexOf(tag, index);
            value = Regex.Split(OFXStream.Substring(ind + tag.Length), "\r\n")[0];
            return value.Trim();
        }

        private string GetTagValue(string tag) => GetTagValue(tag, 0);

        #endregion

        public class OFXTransaction
        {
            public string Type { get; private set; }
            public string DatePosted { get; private set; }
            public string Amount { get; private set; }
            public string Memo { get; private set; }

            public OFXTransaction(string type, string datePosted, string amount, string memo)
            {
                this.Type = type;
                this.DatePosted = datePosted;
                this.Amount = amount;
                this.Memo = memo;
            }
        }
    }
}
