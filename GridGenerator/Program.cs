using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Common;
using Grid.Domain;

namespace GridGenerator
{
    class Program
    {
        private static readonly string VERSION = "1.0";
        private static readonly string ONE_BCC = ConfigurationSettings.AppSettings["OneBcc"] == null ? "false" : ConfigurationSettings.AppSettings["OneBcc"];
        private static readonly string NUM = ConfigurationSettings.AppSettings["NumOfProblemsToGenerate"] == null ? "1" : ConfigurationSettings.AppSettings["NumOfProblemsToGenerate"];
        private static readonly string RETRIES = ConfigurationSettings.AppSettings["NumOfRetries"] == null ? "1000" : ConfigurationSettings.AppSettings["NumOfRetries"];
        private static int num_of_grid_files;
        private static int retries;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                WriteOutRelevantArguments();
                return;
            }
            Dictionary<string, string> splitedArgs = ConsoleAppHelper.SplitArguments(args);

            LoadDefaultArguments(splitedArgs);
            if (!ValidArguments(splitedArgs))
            {
                return;
            }
            num_of_grid_files = int.Parse(splitedArgs["num"]);
            retries = int.Parse(splitedArgs["retries"]);
            bool oneBcc = bool.Parse(splitedArgs["one-bcc"]);

            GridBase generator;
            switch (splitedArgs["type"])
            {
                case "basic":
                    generator = new BasicGenerator(splitedArgs);
                    break;
                case "rooms":
                    generator = new RoomsGenerator(splitedArgs);
                    break;
                case "alternate":
                    generator = new AlternateGenerator(splitedArgs);
                    break;
                default:
                    throw new NotImplementedException();
                    return;
            }


            for (int gridFileId = 1; gridFileId <= num_of_grid_files; gridFileId++)
            {
                int retry_iterations_left = retries;
                while (retry_iterations_left > 0)
                {
                    generator.InitBoard();
                    generator.AddBlockedLocationsStartAndGoal();
                    if (generator.GoalReachable())
                    {

                        string outFileContent = generator.GetGrid();
                        string outFileName = generator.GetFileName(gridFileId, num_of_grid_files);
                        if (oneBcc)
                        {
                            World w = new World(outFileContent, new NoneHeuristic());
                            BiconnectedComponents bcc = new BiconnectedComponents(w);
                            if (bcc.Blocks.Count > 1)
                            {
                                retry_iterations_left--;
                                continue;
                            }

                        }
                        System.IO.File.WriteAllText(outFileName, outFileContent, Encoding.ASCII);
                        break;
                    }
                    retry_iterations_left--;
                }
                if (retries == 0)
                {
                    Console.WriteLine("Couldn't build map for 1000 times - Quiting");
                    return;
                }
            }

        }

        private static bool ValidArguments(Dictionary<string, string> splitedArgs)
        {
            if (!splitedArgs.ContainsKey("type"))
            {
                Console.WriteLine(@"Type is mandatory! - please read the relevant arguments");
                WriteOutRelevantArguments();
                return false;
            }
            else if (!(splitedArgs["type"] == "basic" || splitedArgs["type"] == "rooms" || splitedArgs["type"] == "alternate"))
            {
                Console.WriteLine(@"Wrong type! - please read the relevant arguments");
                WriteOutRelevantArguments();
                return false;
            }

            if (splitedArgs["type"] == "basic")
            {
                if (!(splitedArgs.ContainsKey("basic-blocked") && splitedArgs.ContainsKey("basic-width") &&
                      splitedArgs.ContainsKey("basic-hight") && splitedArgs.ContainsKey("basic-corners")))
                {
                    Console.WriteLine(
                        @"Basic grid must have the following arguments: basic-blocked, basic-width & basic-hight");
                    WriteOutRelevantArguments();
                    return false;
                }
            }

            if (splitedArgs["type"] == "rooms")
            {
                if (!(splitedArgs.ContainsKey("rooms-num-x") && splitedArgs.ContainsKey("rooms-num-y") &&
                      splitedArgs.ContainsKey("rooms-size-x") && splitedArgs.ContainsKey("rooms-size-y") &&
                      splitedArgs.ContainsKey("rooms-door-count-x") && splitedArgs.ContainsKey("rooms-door-count-y") &&
                      splitedArgs.ContainsKey("rooms-door-open-prob") && splitedArgs.ContainsKey("rooms-barier-prob")))
                {
                    Console.WriteLine(@"Rooms grid - one of the arguments is missing - please double check...");
                    WriteOutRelevantArguments();
                    return false;
                }
            }

            if (splitedArgs["type"] == "alternate")
            {
                if (!(splitedArgs.ContainsKey("alternate-blocked-odd") && splitedArgs.ContainsKey("alternate-blocked-even") &&
                      splitedArgs.ContainsKey("alternate-width") && splitedArgs.ContainsKey("alternate-hight") && splitedArgs.ContainsKey("alternate-corners")))
                {
                    Console.WriteLine(@"Alternate grid - one of the arguments is missing - please double check...");
                    WriteOutRelevantArguments();
                    return false;
                }
            }
            return true;
        }

        private static void LoadDefaultArguments(Dictionary<string, string> splitedArgs)
        {
            if (!splitedArgs.ContainsKey("one-bcc")) //default value for one bcc
            {
                splitedArgs.Add("one-bcc", ONE_BCC);
            }

            if (!splitedArgs.ContainsKey("num")) //default value for number of problems
            {
                splitedArgs.Add("num", NUM);
            }

            if (!splitedArgs.ContainsKey("retries")) //default value for number of problems
            {
                splitedArgs.Add("retries", RETRIES);
            }
        }

        private static void WriteOutRelevantArguments()
        {
            Console.WriteLine(@"Grid problems generator");
            Console.WriteLine(@"-=-=-=-=-=-=-=-=-=-=-=-");
            Console.WriteLine(@"Please Provide arguments to run:");
            Console.WriteLine(@"all args should be in the form of: [key]=[value] with space between them");
            Console.WriteLine(@"Arguments:");
            Console.WriteLine(@"----------");
            Console.WriteLine(@"type:                    [basic/rooms/alternate] (mandatory) all other arguments that");
            Console.WriteLine(@"                         start with type name are mandatory for that type ");
            Console.WriteLine(@"num:                     number of problems to generate (default=1)");
            Console.WriteLine(@"retries:                 number of retries before stop generation of grid (default=1000)");
            Console.WriteLine(@"one-bcc:                 [true/false] one bcc in initial state, Not relevant for Rooms (default=false)");
            Console.WriteLine(@"- - - - Type specific args:  All mandatory per type");
            Console.WriteLine(@"basic-blocked:           number of blocked locations");
            Console.WriteLine(@"basic-width:             number - basic size");
            Console.WriteLine(@"basic-hight:             number - basic size");
            Console.WriteLine(@"basic-corners:           [true/false] if true start & goal will be on the top left and bottom right");
            Console.WriteLine(@"                         corners, otherwise they will be random");
            Console.WriteLine(@"- - - -");
            Console.WriteLine(@"rooms-num-x:             number of rooms in the X axis (width)");
            Console.WriteLine(@"rooms-num-y:             number of rooms in the Y axis (hight)");
            Console.WriteLine(@"rooms-size-x:            room size in the X axis (width)");
            Console.WriteLine(@"rooms-size-y:            room size in the Y axis (hight)");
            Console.WriteLine(@"rooms-door-count-x:      number of doors on the X walls (width)");
            Console.WriteLine(@"rooms-door-count-y:      number of doors on the Y walls (hight)");
            Console.WriteLine(@"rooms-door-open-prob:    probability for door to be open");
            Console.WriteLine(@"rooms-barier-prob:       probability for blocked location inside a room");
            Console.WriteLine(@"- - - -");
            Console.WriteLine(@"alternate-width:         number - grid size");
            Console.WriteLine(@"alternate-hight:         number - grid size");
            Console.WriteLine(@"alternate-blocked-odd:   number of blocked odd locations");
            Console.WriteLine(@"alternate-blocked-even:  number of blocked even locations");
            Console.WriteLine(@"alternate-corners:       [true/false] if true start & goal will be on the top left and bottom right");
            Console.WriteLine(@"                         corners, otherwise they will be random");
            Console.WriteLine(@"----------");
            Console.WriteLine(@"Examples:");
            Console.WriteLine(@"Generate 10 basic maps:");
            Console.WriteLine(@"GridGenerator type=basic basic-width=7 basic-hight=5 basic-blocked=9 basic-corners=true num=10");
            Console.WriteLine(@"Generate 5 alternate maps:");
            Console.WriteLine(@"GridGenerator type=alternate alternate-width=5 alternate-hight=5 alternate-blocked-odd=1 alternate-blocked-even=3 alternate-corners=true one-bcc=true num=5");
            Console.WriteLine(@"Generate 3 Rooms maps:");
            Console.WriteLine(@"GridGenerator type=rooms rooms-num-x=2 rooms-num-y=2 rooms-size-x=2 rooms-size-y=2 rooms-door-count-x=1 rooms-door-count-y=1 rooms-door-open-prob=0.9 rooms-barier-prob=0.3 num=3");

            Console.WriteLine(@"-----------------------------[Version:" + VERSION + "]---------------------------------");
            return;
        }
    }
}
