using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBank
{
    public class RawTransaction
    {
        public string Date;
        public string FromAccount;
        public string ToAccount;
        public string Narrative;
        public string Amount;

       public RawTransaction(string Date, string FromAccount, string ToAccount, string Narrative, string Amount)
        {
            this.Date = Date;
            this.FromAccount = FromAccount;
            this.ToAccount = ToAccount;
            this.Narrative = Narrative;
            this.Amount = Amount;
        }

    }
}
