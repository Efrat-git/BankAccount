using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankAccount.Models
{
    public class Account
    {
        private static int accountNumberSeed = 123456;
        private decimal balance;
        private readonly object balanceLock = new object();

        public string Number { get; }
        public List<Transaction> Transactions { get; set; }
        public List<Owner> Owners { get; set; }
        
        public Account()
        {
            Number = accountNumberSeed.ToString();
            accountNumberSeed++;
            
            Transactions = new List<Transaction>();
            Owners = new List<Owner>();
            Owners.Capacity = Owner.OwnersLimit; 
        }

        public decimal GetBalance()
        {
            lock (balanceLock)
            {
                return this.balance;
            }
        }

        public void Deposit(decimal amount, DateTime date)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException($"{date} - deposit {amount} blocked, Amount of deposit must be positive");
            }
            
            lock (balanceLock)
            {
                balance += amount;
                Transactions.Add(new Transaction(amount, date));
                Console.WriteLine($"{date} - deposit {amount} success, current balance {balance}");
            }
        }

        public void Withdrawal(decimal amount, DateTime date)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException($"{date} - withdrawal {amount} blocked, Amount of withdrawal must be positive");
            }
            
            lock (balanceLock)
            {
                if (balance >= amount)
                {
                    balance -= amount;
                    Transactions.Add(new Transaction(-amount, date));
                    Console.WriteLine($"{date} - withdrawal {amount} success, current balance {balance}");
                }
                    
                else
                    throw new InvalidOperationException($"{date} - withdrawal {amount} blocked, Not sufficient funds for this withdrawal");
            }

        }

        public void AddAccountOwner(Owner owner){

            if(CheckOwnerExists(owner.IdNumber))
                throw new Exception("owner already exists for this account");
            if(Owners.Count< Owner.OwnersLimit)
                Owners.Add(owner);
            else
                throw new Exception($"The bank policy is to limit to {Owner.OwnersLimit} owners per account");
        }

        private bool CheckOwnerExists(int ownerId){
            return Owners.Any(x=>x.IdNumber == ownerId); 
        }

        public string GetAccountTransactions()
        {
            if(Transactions.Count == 0)
                return "No transactions";

            var summary = new StringBuilder();
            decimal balance = 0;

            summary.AppendLine("Date\t\t\tAmount\tBalance");

            foreach (var item in Transactions)
            {
                balance += item.Amount;
                summary.AppendLine($"{item.Date}\t{item.Amount}\t{balance}");
            }

            return summary.ToString();
        }

    }
}