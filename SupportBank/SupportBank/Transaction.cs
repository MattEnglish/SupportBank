using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBank
{
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
            from.removeFromAccount(amount, this);
        }



    }
}
