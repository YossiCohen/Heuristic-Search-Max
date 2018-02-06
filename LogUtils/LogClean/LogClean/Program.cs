using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogClean
{

    class Program
    {
        static readonly int LOCATION_OF_PREFIX_TOKEN = 1;
        static readonly string[] LINE_HEADER_TO_REMOVE_BY_CLEAN2 =
        {
            "[SolverStatus"
        };
        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            FileInfo[] fileList = di.GetFiles("*.txt");
    
            foreach (var file in fileList)
            {
                Console.WriteLine("cleanding log:" + file.Name);
                CleanFile(file.Name);
            }

        }

        static void CleanFile(string filename)
        {
            StringBuilder newFileCleanLog = new StringBuilder();
            StringBuilder newFileCleanLog2 = new StringBuilder();
            using (var sr = File.OpenText(filename))
            {
                string line;
                //read file
                while ((line = sr.ReadLine()) != null)
                {
                    string[] items = line.Split(new char[] { ']' },StringSplitOptions.RemoveEmptyEntries);

                    string firstPart = string.Join(" ", items.Take(LOCATION_OF_PREFIX_TOKEN));
                    string secondPart = string.Join("]", items.Skip(LOCATION_OF_PREFIX_TOKEN));
                    //string[] everythingSplitAfterNthOccurence = items.Skip(N_SPACE_TO_REMOVE_TIME).ToArray();
                    newFileCleanLog.Append(secondPart);
                    newFileCleanLog.Append(Environment.NewLine);
                    if (!startsWithOneOfHeaders(secondPart, LINE_HEADER_TO_REMOVE_BY_CLEAN2))
                    {
                        newFileCleanLog2.Append(secondPart);
                        newFileCleanLog2.Append(Environment.NewLine);
                    }
                }
                //write new file
                System.IO.File.WriteAllText(filename.Replace(".txt",".cleanlog"), newFileCleanLog.ToString(), Encoding.UTF8);
                System.IO.File.WriteAllText(filename.Replace(".txt",".cleanlog2"), newFileCleanLog2.ToString(), Encoding.UTF8);
            }
        }

        private static bool startsWithOneOfHeaders(string input, string[] headers)
        {
            bool inputStartsWithOneOfTheHeaders = false;
            foreach (var header in headers)
            {
                inputStartsWithOneOfTheHeaders = input.StartsWith(header);
                if (inputStartsWithOneOfTheHeaders) break;
            }
            return inputStartsWithOneOfTheHeaders;
        }
    }
}
