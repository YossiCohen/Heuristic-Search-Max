using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace ExperimentRunner
{
    class Program
    {
        static object locker = new object();

        private static List<string> headers = new List<string>();
        private static List<string> fileNames = new List<string>();
        private static FileInfo[] grdFileList;
        private static int numOfProcess = int.Parse(ConfigurationSettings.AppSettings["NumberOfGridProcess"] == null ? "2" : ConfigurationSettings.AppSettings["NumberOfGridProcess"]);
        private static bool retryStoppedByTime = bool.Parse(ConfigurationSettings.AppSettings["RetryStoppedByTime"] == null ? "false" : ConfigurationSettings.AppSettings["RetryStoppedByTime"]);

        private static Dictionary<string, Dictionary<string, string>> logsData =
            new Dictionary<string, Dictionary<string, string>>();

        //<Problem,<Algorithm,<Heuristic,<BccInit,<Prunning,HowEnded>>>>>
        private static Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>> existingLoggedRuns = 
            new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>>();

        //<file,args>
        private static List<FullCommand> commandToBeDone = new List<FullCommand>();
        private static List<string[]> profileArgs = new List<string[]>();

        static void Main(string[] args)
        {
            if (!InitPass())
            {
                return;
            }
            Console.WriteLine($"Running experiments with {numOfProcess} paralel processes");
            LoadExistingLogsAndRemoveGarbage();
            createNeededCommands();
            StartNewCommand(null,null);
            var gridProcceses = Process.GetProcessesByName("Grid");
            while (commandToBeDone.Count > 0 || gridProcceses.Length > 0)
            {
                Thread.Sleep(3000);
                gridProcceses = Process.GetProcessesByName("Grid");
            }
            Console.WriteLine("All Done, run this again to be sure...");
        }

        private static void StartNewCommand(object sender, System.EventArgs e)
        {
            lock (locker) // Thread safe code
            {
                if (sender != null)
                {
                    Console.WriteLine($"Grid.exe Exited: {((Process)sender).StartInfo.Arguments} (More runs to go: {commandToBeDone.Count})");
                }
                var gridProcceses = Process.GetProcessesByName("Grid");
                for(int i = gridProcceses.Length; i<numOfProcess;i++)
                {
                    if (commandToBeDone.Count > 0)
                    {
                        startProcessWithCommand(commandToBeDone[0]);
                        commandToBeDone.RemoveAt(0);
                    }
                }

            }
        }

        private static void startProcessWithCommand(FullCommand fullCommand)
        {
            var p = new Process();
            p.StartInfo.FileName = "Grid.exe";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.Arguments = $"problem={fullCommand.filename} {fullCommand.args[0]} {fullCommand.args[1]} {fullCommand.args[2]} {fullCommand.args[3]}";
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.CreateNoWindow = false;
            p.EnableRaisingEvents = true;
            p.Exited += StartNewCommand;
            p.Start();
            p.PriorityClass = ProcessPriorityClass.RealTime;
            Console.WriteLine($"Grid.exe Started: {p.StartInfo.Arguments}");
        }
        


        static bool InitPass()
        {
            DirectoryInfo currentDirectoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            grdFileList = currentDirectoryInfo.GetFiles("*.grd");
            if (grdFileList.Length == 0)
            {
                Console.WriteLine("no *.grd files here, bye!");
                return false;
            }
            if (!File.Exists("Profile.txt"))
            {
                Console.WriteLine("File: Profile.txt missing. bye!");
                return false;
            }
            var profile  = File.ReadAllLines("Profile.txt");
            foreach (var line in profile)
            {
                if (line.StartsWith("//"))
                {
                    continue;
                }
                profileArgs.Add(line.Split(' '));
            }
            //Kill all Grid.exe
            var gridProcceses = Process.GetProcessesByName("Grid");
            foreach (var gridProccese in gridProcceses)
            {
                gridProccese.Kill();
            }
            return true;
        }

        private static void LoadExistingLogsAndRemoveGarbage()
        {
            var runningLogsFolder = Path.Combine(Directory.GetCurrentDirectory(), "RunningLogs");
            if (!Directory.Exists(runningLogsFolder))
            {
                return;
            }
            DirectoryInfo di = new DirectoryInfo(runningLogsFolder);
            FileInfo[] fileList = di.GetFiles("*.txt");
            foreach (var file in fileList)
            {
                ExtractHeadersAndKVP(file.ToString());
                //Delete file if have no HowEnded key
                if (!logsData[file.Name].Keys.Contains("HowEnded"))
                {
                    Console.Out.WriteLine($"File: {file.Name}, have no HowEnded line - KILLING!!!");
                    file.Delete();
                }
            }
            foreach (var logItem in logsData)
            {
                if (!logItem.Value.Keys.Contains("HowEnded"))
                {
                    continue;
                }
                if (!existingLoggedRuns.Keys.Contains(logItem.Value["Problem"].ToLower()))
                {
                    existingLoggedRuns.Add(logItem.Value["Problem"].ToLower(), new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, string>>>>());
                }
                if (!existingLoggedRuns[logItem.Value["Problem"].ToLower()].Keys.Contains(logItem.Value["Algorithm"]))
                {
                    existingLoggedRuns[logItem.Value["Problem"].ToLower()].Add(logItem.Value["Algorithm"], new Dictionary<string, Dictionary<string, Dictionary<string, string>>>());
                }
                if (!existingLoggedRuns[logItem.Value["Problem"].ToLower()][logItem.Value["Algorithm"]].Keys.Contains(logItem.Value["Heuristic"]))
                {
                    existingLoggedRuns[logItem.Value["Problem"].ToLower()][logItem.Value["Algorithm"]].Add(logItem.Value["Heuristic"], new Dictionary<string, Dictionary<string, string>>());
                }
                if (!existingLoggedRuns[logItem.Value["Problem"].ToLower()][logItem.Value["Algorithm"]][logItem.Value["Heuristic"]].Keys.Contains(logItem.Value["BccInit"]))
                {
                    existingLoggedRuns[logItem.Value["Problem"].ToLower()][logItem.Value["Algorithm"]][logItem.Value["Heuristic"]].Add(logItem.Value["BccInit"], new Dictionary<string, string>());
                }
                existingLoggedRuns[logItem.Value["Problem"].ToLower()][logItem.Value["Algorithm"]][logItem.Value["Heuristic"]][logItem.Value["BccInit"]].Add(logItem.Value["Prunning"], logItem.Value["HowEnded"]);

            }
        }

        private static void createNeededCommands()
        {
            foreach (var file in grdFileList)
            {
                string alg = "", heuristic = "", bccinit = "", prune = "";
                foreach (var profileArg in profileArgs)
                {
                    foreach (var str in profileArg)
                    {
                        var kv = str.Split('=');
                        kv[1] = kv[1].ToLower();
                        if (kv[0] == "alg")
                        {
                            switch (kv[1])
                            {
                                case "astar":
                                    alg = "AStarMax";
                                    break;
                                case "dfbnb":
                                    alg = "DfBnbMax";
                                    break;
                            }
                        }
                        if (kv[0] == "prune")
                        {
                            switch (kv[1])
                            {
                                case "none":
                                    prune = "NoPrunning";
                                    break;
                                case "bsd":
                                    prune = "BasicSymmetryDetectionPrunning";
                                    break;
                                case "rsd":
                                    prune = "ReachableSymmetryDetectionPrunning";
                                    break;
                            }
                        }
                        if (kv[0] == "heuristic")
                        {
                            switch (kv[1])
                            {
                                case "none":
                                    heuristic = "NoneHeuristic";
                                    break;
                                case "untouched":
                                    heuristic = "UntouchedAroundTheGoalHeuristic";
                                    break;
                                case "bcc":
                                    heuristic = "BiconnectedComponentsHeuristic";
                                    break;
                                case "alternate":
                                    heuristic = "AlternateStepsHeuristic";
                                    break;
                                case "altbcc":
                                    heuristic = "AlternateStepsBiconnectedComponentsHeuristic";
                                    break;
                                case "sepaltbcc":
                                    heuristic = "SeparateAlternateStepsBiconnectedComponentsHeuristic";
                                    break;
                            }
                        }
                        if (kv[0] == "bcc-init")
                        {
                            switch (kv[1])
                            {
                                case "true":
                                    bccinit = "True";
                                    break;
                                case "false":
                                    bccinit = "False";
                                    break;
                            }
                        }
                    }

                    bool needToAdd = true;
                    if (existingLoggedRuns.Keys.Contains(file.Name.ToLower()))
                    {
                        if (existingLoggedRuns[file.Name.ToLower()].Keys.Contains(alg))
                        {
                            if (existingLoggedRuns[file.Name.ToLower()][alg].Keys.Contains(heuristic))
                            {
                                if (existingLoggedRuns[file.Name.ToLower()][alg].Keys.Contains(heuristic))
                                {
                                    if (existingLoggedRuns[file.Name.ToLower()][alg][heuristic].Keys.Contains(bccinit))
                                    {
                                        if (existingLoggedRuns[file.Name.ToLower()][alg][heuristic][bccinit].Keys
                                            .Contains(prune))
                                        {
                                            var howEnded =
                                                existingLoggedRuns[file.Name.ToLower()][alg][heuristic][bccinit][prune];
                                            switch (howEnded)
                                            {
                                                case "Searching":
                                                    break;
                                                case "Ended":
                                                    needToAdd = false;
                                                    break;
                                                case "StoppedByTime":
                                                    needToAdd = retryStoppedByTime;
                                                    Console.Out.WriteLine($"StoppedByTime: {file}, {alg}, {prune}");
                                                    break;
                                                case "IllegalStartState":
                                                    Console.Out.WriteLine($"IllegalStartState: {file}, {alg}, {prune}");
                                                    break;
                                                case "StoppedByMemoryLimit":
                                                    Console.Out.WriteLine(
                                                        $"StoppedByMemoryLimit: {file}, {alg}, {prune}");
                                                    break;
                                            }
                                        }

                                    }
                                }
                            }
                        }

                    }


                    if (needToAdd)
                    {
                        var cmd = new FullCommand();
                        cmd.filename = file.Name;
                        cmd.args = profileArg;
                        commandToBeDone.Add(cmd);
                    }
                }

            }
        }

        private static void ExtractHeadersAndKVP(string file)
        {
            Console.Out.WriteLine("Reading log:" + file);
            fileNames.Add(file);
            logsData.Add(file, new Dictionary<string, string>());
            string contents = File.ReadAllText("RunningLogs\\" + file);
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
                logsData[file].Add(kvp[0], kvp[1]);
            }
        }

        struct FullCommand
        {
            public string filename;
            public string[] args;
        }
    }
}
