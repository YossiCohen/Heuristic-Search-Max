using System;
using System.Collections.Generic;

namespace Common
{
    public class ConsoleAppHelper
    {
        public static Dictionary<string, string> SplitArguments(string[] args)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var s in args)
            {
                var splitted = s.Split('=');
                if (splitted.Length != 2)
                {
                    Console.WriteLine("WAIT! I can't understand:" + s);
                    throw new ArgumentException("Bad Argument" + s);
                }
                result.Add(splitted[0].ToLower(), splitted[1].ToLower());
            }
            return result;
        }
    }
}
