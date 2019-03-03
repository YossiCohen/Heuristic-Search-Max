using System;
using System.Collections;
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

            ExportSortedResults();
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

        public static void ExportSortedResults()
        {
            //NOTICE! this method assumes that all the DATA is loaded already

            //pass 1 - arrange the data
            Dictionary<string, List<int>> HeuristicPruningExpanded = new Dictionary<string, List<int>>();
            Dictionary<string, List<double>> HeuristicPruningTime = new Dictionary<string, List<double>>();
            List<string> headers = new List<string>();
            foreach (var fn in fileNames) 
            {
                if (data[fn]["HowEnded"] != "Ended")
                {
                    continue;
                }
                var heuristic = data[fn]["Heuristic"];
                var pruning = data[fn]["Prunning"];
                var heuristicPruning = heuristic + "_" + pruning;
                var expanded = int.Parse(data[fn]["Expended"]);
                var time = double.Parse(data[fn]["TotalTime(MS)"]);
                if (!headers.Contains(heuristic))
                {
                    headers.Add(heuristic);
                }
                if (!headers.Contains(pruning))
                {
                    headers.Add(pruning);
                }
                if (!headers.Contains(heuristicPruning))
                {
                    headers.Add(heuristicPruning);
                }
                if (!HeuristicPruningExpanded.ContainsKey(heuristic))
                {
                    HeuristicPruningExpanded.Add(heuristic, new List<int>());
                    HeuristicPruningTime.Add(heuristic, new List<double>());
                }
                if (!HeuristicPruningExpanded.ContainsKey(pruning))
                {
                    HeuristicPruningExpanded.Add(pruning, new List<int>());
                    HeuristicPruningTime.Add(pruning, new List<double>());
                }
                if (!HeuristicPruningExpanded.ContainsKey(heuristicPruning))
                {
                    HeuristicPruningExpanded.Add(heuristicPruning, new List<int>());
                    HeuristicPruningTime.Add(heuristicPruning, new List<double>());
                }
                HeuristicPruningExpanded[heuristic].Add(expanded);
                HeuristicPruningTime[heuristic].Add(time);
                HeuristicPruningExpanded[pruning].Add(expanded);
                HeuristicPruningTime[pruning].Add(time);
                HeuristicPruningExpanded[heuristicPruning].Add(expanded);
                HeuristicPruningTime[heuristicPruning].Add(time);
            }

            //Pass 2 : output
            if (headers.Count > 1)
            {
                StringBuilder sortedResultsExpanded = new StringBuilder();
                StringBuilder sortedResultsTime = new StringBuilder();
                //init csv headers and sort
                for (int i = 0; i < headers.Count; i++)
                {
                    sortedResultsExpanded.Append(headers[i] + "," );
                    sortedResultsTime.Append(headers[i] + ",");
                    HeuristicPruningTime[headers[i]].Sort();
                    HeuristicPruningExpanded[headers[i]].Sort();
                }
                sortedResultsExpanded.Append(Environment.NewLine);
                sortedResultsTime.Append(Environment.NewLine);

                bool atleastOneFound = true;
                var listIndex = 0;
                while (atleastOneFound)
                {
                    atleastOneFound = false;
                    listIndex++;
                    for (int i = 0; i < headers.Count; i++)
                    {
                        if (HeuristicPruningExpanded[headers[i]].Count > listIndex)
                        {
                            atleastOneFound = true;
                            sortedResultsExpanded.Append(HeuristicPruningExpanded[headers[i]][listIndex]);
                            sortedResultsTime.Append(HeuristicPruningTime[headers[i]][listIndex]);
                        }
                        sortedResultsExpanded.Append(",");
                        sortedResultsTime.Append(",");
                    }
                    sortedResultsExpanded.Append(Environment.NewLine);
                    sortedResultsTime.Append(Environment.NewLine);
                }

                File.WriteAllText(GetTimestampFileName(DateTime.Now, "PerHeuristicAndPruningResults-Expanded"), sortedResultsExpanded.ToString());
                File.WriteAllText(GetTimestampFileName(DateTime.Now, "PerHeuristicAndPruningResults-Time"), sortedResultsTime.ToString());
            }

        }
    }

}