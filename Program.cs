using System;
using System.Threading.Tasks;
using BankAccount.Models;
using System.Linq;
using System.Collections.Generic;

namespace BankAccount
{
    class Program
    {
        private static List<Account> accounts;

        static void Main(string[] args)
        { 
            Init();
            ShowGeneralMenu();
        }

        private static void Init(){
            accounts = new List<Account>();            
        }

        private static void ShowGeneralMenu()
        { 
            int choice;

            while(true)
            {
                Console.WriteLine("\n********Welcome To Bank********");
                Console.WriteLine("1. Create an account");
                Console.WriteLine("2. Connect to an account");
                Console.WriteLine("3. Simulate account synchronization");
                Console.WriteLine("**************************\n");
                Console.WriteLine("Enter your choice: ");
                choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                    {
                        Console.WriteLine("enter owner id");
                        int ownerId = int.Parse(Console.ReadLine());
                        Console.WriteLine("enter owner first name");
                        string firstName = Console.ReadLine();
                        Console.WriteLine("enter owner last name");
                        string lastName = Console.ReadLine();

                        var owner = new Owner(ownerId, firstName, lastName);
                        var account = new Account();
                        account.AddAccountOwner(owner);    
                        accounts.Add(account);

                        Console.WriteLine($"Your account created successfully, account number is : {account.Number} ");   
                    }break;
                    case 2:
                    {
                        Console.WriteLine("Enter your account number");
                        var number = Console.ReadLine().Trim();
                        
                        var account = accounts.Find(x=> x.Number == number);                       
                        if(account == null){
                            Console.WriteLine("There is no account with such a number");
                            break;
                        }

                        Console.WriteLine("Enter your id number");
                        var id = int.Parse(Console.ReadLine());
                        
                        if(!account.Owners.Any(x=>x.IdNumber == id)) 
                        {
                            Console.WriteLine("You are not the account owner");
                            break;
                        }  
                        else
                        {
                            Console.WriteLine($"Connect to {account.Number} account");
                            ShowAccountMenu(account);
                        }
                    }break;
                    case 3:
                    {
                        Task.Run(() => SimulateAccountSynchronization()).Wait();
                    }break;  
                }
                Console.Write("Enter to continue");
                Console.ReadLine();
            }
        }

        private static void ShowAccountMenu(Account account)
        { 
            int choice;
            decimal amount;
            var showMenu = true;

            while (showMenu)
            {
                Console.WriteLine($"\n***** Account {account.Number} *****");
                Console.WriteLine("1. Check balance");
                Console.WriteLine("2. Withdraw");
                Console.WriteLine("3. Deposit");
                Console.WriteLine("4. Add account owner");
                Console.WriteLine("5. All transactions" );
                Console.WriteLine("6. Exit");
                Console.WriteLine("**************************\n");
                Console.WriteLine("Enter your choice: ");
                
                choice = int.Parse(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine($"Your balance is : {account.GetBalance()} ");
                            break;
                        case 2:
                        {
                            Console.WriteLine("enter the amount to withdrow: ");
                            amount = decimal.Parse(Console.ReadLine());
                            account.Withdrawal(amount, DateTime.Now);
                        }
                        break;
                        case 3:
                        {
                            Console.WriteLine("enter the amount to deposit: ");
                            amount = decimal.Parse(Console.ReadLine());
                            account.Deposit(amount, DateTime.Now);
                        }
                        break;
                        case 4:
                        {
                            Console.WriteLine("enter owner id");
                            int ownerId = int.Parse(Console.ReadLine());
                            Console.WriteLine("enter owner first name");
                            string firstName = Console.ReadLine();
                            Console.WriteLine("enter owner last name");
                            string lastName = Console.ReadLine();

                            var owner = new Owner(ownerId, firstName, lastName);
                            account.AddAccountOwner(owner);
                            Console.WriteLine("owner added successfully");
                        }break;
                        case 5:
                        {
                            Console.WriteLine(account.GetAccountTransactions());
                        }break;
                        case 6:
                        {
                            Console.WriteLine("Good bye");
                            showMenu = false;
                        }break;
                    }
                }
                catch (Exception ex)
                {
                  Console.WriteLine(ex.Message);  
                }

                Console.Write("Enter to continue");
                Console.ReadLine();
            }
        }
        
        static async Task SimulateAccountSynchronization()
        {
            var account = new Account();
            account.Deposit(10, DateTime.Now);
            var tasks = new Task[2];
            
            for (int i = 0; i < tasks.Length; i++)
                tasks[i] = Task.Run(() => Update(account));

            await Task.WhenAll(tasks);
            Console.WriteLine($"Account's balance is {account.GetBalance()}");
        }

        static void Update(Account account)
        {
            decimal[] amounts = { -10, -15, 25, 40, -20, -100};
            foreach (var amount in amounts)
            { 
                try
                {
                    if (amount >= 0)
                        account.Deposit(amount, DateTime.Now);
                    else
                        account.Withdrawal(Math.Abs(amount), DateTime.Now);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
        }
    }
}
