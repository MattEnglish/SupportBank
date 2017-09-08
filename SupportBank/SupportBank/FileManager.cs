using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;

namespace SupportBank
{
    public static class FileManager
    {
        

        public static List<RawTransaction> GetRawTransactionsFromJson(string filePath)
        {
            return JsonConvert.DeserializeObject<List<RawTransaction>>(File.ReadAllText(filePath));           
        }
    }
}
