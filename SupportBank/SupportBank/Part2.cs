using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace SupportBank
{
    public class Part2
    {
        public static void Run()
        {           
            Part1.Run("Transactions2015.csv");
        }
    }
}
