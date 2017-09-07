using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace SupportBank
{
    
    public class Part1
    {
        enum Commands {invalid,ListAll,ListName}
        public static void Run()
        {
            string command = "";

            while (command != "exit")
            {
                GenerateTransactionsFromFile("Transactions.csv");
                command = Console.ReadLine().ToLower();
                DoCommand(command);
            } 
        }
        
        private static void DoCommand(String commandString)
        {
            Commands command = getCommand(commandString);
            if(commandString == "exit")
            {
                return;
            }
            if (command == Commands.ListName)
            {
                foreach (Person person in People.people)
                {
                    if (commandString.Substring(5).ToLower() == person.name.ToLower())
                    {
                        PrintAllTransactions(person);
                        return;
                    }
                }
                Console.WriteLine("invalid name");
                return;
            }
            else if (command == Commands.ListAll)
            {
                PrintAllAccounts();
                return;
            }
            Console.WriteLine("Invalid Command");
            return;
        }
        private static Commands getCommand(string command)
        {
            command = command.ToLower();
            if (command.Length < 5)
            {
                return Commands.invalid;
            }

            if (command.Substring(0, 5).ToLower() == "list ")
            {
                if (command.Substring(5, 3) == "all")
                {
                    return Commands.ListAll;
                }
                return Commands.ListName;
            }
            return Commands.invalid;
        }



        private static void GenerateTransactionsFromFile(string filePath)
        {
            var transactions = File.ReadAllLines(filePath);
            for (int i = 1; i < transactions.Length; i++)
            {
                string[] a = transactions[i].Split(',');
                decimal amount = Convert.ToDecimal(a[4]);
                if (!People.doesPersonExist(a[1]))
                {
                    new Person(a[1]);
                }
                if (!People.doesPersonExist(a[2]))
                {
                    new Person(a[2]);
                }
                Person from = People.GetPersonWithName(a[1]);
                Person to = People.GetPersonWithName(a[2]);
                string date = a[0];
                string narrative = a[3];
                Transaction transaction = new Transaction(date, amount, from, to, narrative);
                transaction.applyTransaction();
            }

        }
        public static void PrintAllAccounts()
        {
            foreach (Person p in People.people)
            {
                Console.WriteLine("{0}     \t {1}", p.name, p.GetAccount());
            }
        }
        
        public static void PrintAllTransactions(Person person)
        {
            foreach (Transaction transaction in person.GetTransactionHistory())
            {
                Console.WriteLine("{0}     \t {1}", transaction.date, transaction.narrative);
            }
            
        }
        


    }

    public class Transaction
    {       
        public decimal amount { get; }
        public Person from { get; }
        public Person to { get; }
        public string narrative { get; }
        public string date { get; }

        public Transaction(string theDate, decimal amount, Person from, Person to, string narrative)
        {
            this.amount = amount;
            this.from = from;
            this.to = to;
            this.date = theDate;
            this.narrative = narrative;
        }

        public void applyTransaction()
        {
            to.addToAccount(amount, this);
            from.removeFromAccount(amount , this);
        }

        
            
    }

    public class Person
    {
        
        public string name { get; }
        public decimal account { set; get; }
        new List<Transaction> transactionHistory = new List<Transaction>();

        public Person(string name)
        {
            this.name = name;
            People.people.Add(this);
        }

        public decimal GetAccount()
        {
            return account;
        }


        public void addToAccount(decimal amount,Transaction transaction)
        {
            this.account += amount;
            transactionHistory.Add(transaction);
        }

        public void removeFromAccount(decimal amount, Transaction transaction)
        {
            this.account -= amount;
            transactionHistory.Add(transaction);

        }

        public List<Transaction> GetTransactionHistory()
        {
            return transactionHistory;
        }

    }

    
}
