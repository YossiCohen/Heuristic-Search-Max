using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExpSum
{
    class Program
    {
        private static Dictionary<string, Dictionary<string,string>> data = new Dictionary<string, Dictionary<string, string>>();
        private static List<string> headers = new List<string>();
        private static List<string> fileNames = new List<string>(); 

        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            FileInfo[] fileList = di.GetFiles("*.txt");

            foreach (var file in fileList)
            {
                ExtractHeadersAndKVP(file.ToString());
            }
            
            StringBuilder sb = new StringBuilder();
            sb.Append("FileName,");
            foreach (var header in headers)
            {
                sb.Append(header);
                sb.Append(",");
            }
            sb.Append(Environment.NewLine);

            foreach (var fn in fileNames)
            {
                sb.Append(fn);
                sb.Append(",");
                foreach (var header in headers)
                {
                    if (data[fn].ContainsKey(header))
                    {
                        sb.Append(data[fn][header]);                        
                    }
                    sb.Append(",");
                }
                sb.Append(Environment.NewLine);
            }

            File.WriteAllText(GetTimestampFileName(DateTime.Now),sb.ToString());
        }

        public static string GetTimestampFileName(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssfff")+".csv";
        }

        private static void ExtractHeadersAndKVP(string file)
        {
            Console.Out.WriteLine("Handeling:" + file);
            fileNames.Add(file);
            data.Add(file, new Dictionary<string, string>());
            string contents = File.ReadAllText(file);
            var pairs = Regex.Matches(contents, @"\[\[(.+?)\]\]")
                             .Cast<Match>()
                             .Select(m => m.Groups[1].Value)
                             .ToList();

            foreach (var pair in pairs)
            {
                string[] kvp = pair.Split(':');
                if (!headers.Contains(kvp[0]))
                {
                    headers.Add(kvp[0]);
                }
                data[file].Add(kvp[0],kvp[1]);
            }
        }
    }
}
