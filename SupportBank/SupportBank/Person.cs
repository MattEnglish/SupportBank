using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBank
{
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


        public void addToAccount(decimal amount, Transaction transaction)
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
