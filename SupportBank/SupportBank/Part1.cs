using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using NLog;
using NLog.Config;
using NLog.Targets;


namespace SupportBank
{

    public class Part1
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();


        public static People supportTeam = new People();
        public static People corruptedPeople = new People();
        public static List<Transaction> transactionList = new List<Transaction>();

        enum Commands
        {
            invalid,
            ListAll,
            ListName
        }

        public static void Run()
        {

            Run("Transactions.csv");
        }

        public static void Run(string filePath)
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget { FileName = @"C:\Work\Training\Logs\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;

            string command = "";
            GenerateTransactionsFromFile(filePath);

            while (command != "exit")
            {               
                command = Console.ReadLine().ToLower();
                DoCommand(command);
            }
        }


        private static void DoCommand(String commandString)
        {
            Commands command = getCommand(commandString);
            if (commandString == "exit")
            {
                return;
            }
            if (command == Commands.ListName)
            {
                foreach (Person person in supportTeam.people)
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
                string[] transactionLine = transactions[i].Split(',');
                decimal amount = 0;
                Person from = GetPersonWithNameAndAddToSupportTeam(transactionLine[1]);
                Person to = GetPersonWithNameAndAddToSupportTeam(transactionLine[2]);
                string date = transactionLine[0];
                string narrative = transactionLine[3];
                try
                {
                    amount = Convert.ToDecimal(transactionLine[4]);

                }
                catch(SystemException)
                {
                    var message = "transaction  " + transactionLine[0] + ", " + transactionLine[1] + ", " + transactionLine[2] + ", " + transactionLine[3] + ", " + transactionLine[4]
                        + ". \n" + transactionLine[4] + " should be a number";
                    var info = LogEventInfo.Create(LogLevel.Error, "transaction amount logger", message);
                    logger.Log(info);
                    Console.Write("There is an error in the Transactions");
                    Console.WriteLine(message);
                    Console.WriteLine("AllTransaction involving {0} and {1} have been removed", transactionLine[1], transactionLine[2]);
                    corruptedPeople.people.Add(from);
                    corruptedPeople.people.Add(to);
                }
                transactionList.Add(new Transaction(date, amount, from, to, narrative));

            }
            transactionList.RemoveAll(TransactionContainsCorruptPerson);
            foreach(Transaction t in transactionList)
            {
                t.ApplyTransaction();
            }

            



        }

        private static bool TransactionContainsCorruptPerson(Transaction transaction)
        {
            foreach (Person person in corruptedPeople.people)
            {
             if(transaction.from == person || transaction.to == person)
                {
                    return true;
                }

            }
            return false;
        }


        private static Person GetPersonWithNameAndAddToSupportTeam(string name)
        {
            if (!supportTeam.DoesPersonExist(name))
            {
                supportTeam.people.Add(new Person(name));
            }

            return supportTeam.GetPersonWithName(name);
        }

        public static void PrintAllAccounts()
        {
            foreach (Person p in supportTeam.people)
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






}
