using System;

namespace BankAccount.Models
{
    public class Transaction
    {
        public decimal Amount { get; }
        public DateTime Date { get; }

        public Transaction(decimal amount, DateTime date)
        {
            this.Amount = amount;
            this.Date = date;
        }

        
    }
}