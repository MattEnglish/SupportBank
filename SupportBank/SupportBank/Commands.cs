using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBank
{
    public class CommandReader
    {




        public static Commands getCommand(string command)
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

            if(command.Length> 7 && command.Substring(0,6).ToLower() == "import")
            {
                return Commands.ImportFile;
            }

            return Commands.invalid;
        }

    }
}
