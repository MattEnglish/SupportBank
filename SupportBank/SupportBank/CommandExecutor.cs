using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SupportBank
{
    public class CommandExecutor
    {
        public static void DoCommand(String commandString, People supportTeam)
        {
            Commands command = CommandReader.getCommand(commandString);
            if (commandString == "exit")
            {
                return;
            }
            if (command == Commands.ListName)
            {
                foreach (Person person in supportTeam.people)
                {
                    string name = person.name.ToLowerInvariant();
                    string name2 = commandString.Substring(5).ToLowerInvariant();
                    if (name2 == name)
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
                PrintAllAccounts(supportTeam);
                return;
            }
            else if (command == Commands.ImportFile)
            {
                LoadNewFile(commandString);
                return;
            }
            Console.WriteLine("Invalid Command");
            return;
        }

        public static void PrintAllAccounts(People supportTeam)
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

        public static void LoadNewFile(string commandString)
        {
            string filePath = commandString.Substring(7);
            if (!File.Exists(filePath))
            {               
                Console.WriteLine("Cannot find file");
                return;
            }
            string fileExstension = commandString.Substring(commandString.Length - 4).ToLower();
            
            if(fileExstension == "json")
            {
                var rawTransactions = FileManager.GetRawTransactionsFromJson(filePath);
                Part1.GenerateTransactionsFromRaw(rawTransactions);    
                Part1.getAndFollowCommand();
            }
            else if(fileExstension == ".csv")
            {
                Part1.Run(filePath);
            }
            else
            {
                Console.WriteLine("invalid filetype");
                return;
            }
        }

    }
}
