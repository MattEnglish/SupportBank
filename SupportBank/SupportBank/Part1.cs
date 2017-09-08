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
using Newtonsoft;


namespace SupportBank
{
    public enum Commands
    {
        invalid,
        ListAll,
        ListName,
        ImportFile
    }

    public class Part1
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static People supportTeam = new People();
        public static People corruptedPeople = new People();
        

        

        public static void Run()
        {
            Run("Transactions.csv");
        }

        public static void Run(string filePath)
        {
            GenerateTransactionsFromRaw(GenerateRawTransactionsFromcsvFile(filePath));
            getAndFollowCommand();
        }

        public static void getAndFollowCommand()
        {
            string command = "";
            while (command != "exit")
            {
                command = Console.ReadLine().ToLower();
                CommandExecutor.DoCommand(command, supportTeam);
            }
        }


        public static void Logging()
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget { FileName = @"C:\Work\Training\Logs\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;
        }
        
        

        private static List<RawTransaction> GenerateRawTransactionsFromcsvFile(string filePath)
        {
            var transactions = File.ReadAllLines(filePath);
            var rawTransactions = new List<RawTransaction>();
            for (int i = 1; i < transactions.Length; i++)
            {
                string[] tl = transactions[i].Split(',');

                rawTransactions.Add(new RawTransaction(tl[0], tl[1], tl[2], tl[3], tl[4]));
            }
            return rawTransactions;
        }

        public static List<Transaction> GenerateTransactionsFromRaw(List<RawTransaction> rawTransactions)
        {
            var transactionList = new List<Transaction>();
            supportTeam = new People();
            foreach (var rawTransaction in rawTransactions)
            {
                Person from = GetPersonWithNameAndAddToSupportTeam(rawTransaction.FromAccount);
                Person to = GetPersonWithNameAndAddToSupportTeam(rawTransaction.ToAccount);
                decimal amount = 0;
                try
                {
                    amount = Convert.ToDecimal(rawTransaction.Amount);

                }
                catch (SystemException)
                {
                    var message = "transaction  " + rawTransaction.Date + ", " + rawTransaction.FromAccount + ", " + rawTransaction.ToAccount + ", " + rawTransaction.Narrative + ", " + rawTransaction.Amount
                        + ". \n" + rawTransaction.Amount + " should be a number";
                    var info = LogEventInfo.Create(LogLevel.Error, "transaction amount logger", message);
                    logger.Log(info);
                    Console.Write("There is an error in the Transactions");
                    Console.WriteLine(message);
                    Console.WriteLine("AllTransaction involving {0} and {1} have been removed", rawTransaction.FromAccount, rawTransaction.ToAccount);
                    corruptedPeople.people.Add(from);
                    corruptedPeople.people.Add(to);
                    
                }
                transactionList.Add(new Transaction(rawTransaction.Date, amount, from, to, rawTransaction.Narrative));

            }

            transactionList.RemoveAll(TransactionContainsCorruptPerson);
            foreach (Transaction t in transactionList)
            {
                t.ApplyTransaction();
            }
            return transactionList;

        }

    

                

            
            





        

        private static bool TransactionContainsCorruptPerson(Transaction transaction)
        {
            foreach (Person person in corruptedPeople.people)
            {
                if (transaction.from == person || transaction.to == person)
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

        



    }






}
