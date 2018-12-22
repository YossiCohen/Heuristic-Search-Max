using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExpSum
{
    class Program
    {
        private static Dictionary<string, Dictionary<string, string>> data =
            new Dictionary<string, Dictionary<string, string>>();

        private static List<string> headers = new List<string>();
        private static List<string> fileNames = new List<string>();
        private static Dictionary<string, bool> problemEndedForAll = new Dictionary<string, bool>();

        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            FileInfo[] fileList = di.GetFiles("*.txt");

            foreach (var file in fileList)
            {
                ExtractHeadersAndKVP(file.ToString());
            }

            StringBuilder sbAll = new StringBuilder();
            StringBuilder sbProblemEndedForAll = new StringBuilder();
            sbAll.Append("FileName,");
            sbProblemEndedForAll.Append("FileName,");
            foreach (var header in headers)
            {
                sbAll.Append(header);
                sbAll.Append(",");
                sbProblemEndedForAll.Append(header);
                sbProblemEndedForAll.Append(",");
            }
            sbAll.Append(Environment.NewLine);
            sbProblemEndedForAll.Append(Environment.NewLine);

            foreach (var fn in fileNames)  //Pass 1: store all & figure out what problems are solved via all solvers
            {
                sbAll.Append(fn);
                sbAll.Append(",");
                foreach (var header in headers)
                {
                    if (data[fn].ContainsKey(header))
                    {
                        sbAll.Append(data[fn][header]);
                    }
                    sbAll.Append(",");
                }
                sbAll.Append(Environment.NewLine);
                if (!problemEndedForAll.ContainsKey(data[fn]["Problem"]))
                { 
                    problemEndedForAll.Add(data[fn]["Problem"], true);
                }
                bool ended = data[fn].ContainsKey("HowEnded") && data[fn]["HowEnded"] == "Ended";
                problemEndedForAll[data[fn]["Problem"]] = problemEndedForAll[data[fn]["Problem"]] && ended;
            }

            File.WriteAllText(GetTimestampFileName(DateTime.Now, "All"), sbAll.ToString());

            foreach (var fn in fileNames)  //Pass 2: only problems that are solved via all solvers
            {
                if (!problemEndedForAll[data[fn]["Problem"]])
                {
                    continue;
                }
                sbProblemEndedForAll.Append(fn);
                sbProblemEndedForAll.Append(",");
                foreach (var header in headers)
                {
                    if (data[fn].ContainsKey(header))
                    {
                        sbProblemEndedForAll.Append(data[fn][header]);
                    }
                    sbProblemEndedForAll.Append(",");
                }
                sbProblemEndedForAll.Append(Environment.NewLine);
            }
            File.WriteAllText(GetTimestampFileName(DateTime.Now, "SolvedByAll"), sbProblemEndedForAll.ToString());
        }

        public static string GetTimestampFileName(DateTime value, string summaryType)
        {
            return value.ToString("yyyyMMddHHmmssfff") +"-" + summaryType + ".csv";
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
                data[file].Add(kvp[0], kvp[1]);
            }
        }
    }
}