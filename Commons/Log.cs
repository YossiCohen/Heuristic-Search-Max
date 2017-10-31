using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Common
{
    public static class Log
    {
        public enum FlowMarker
        {
            EnterMethod,ExitMethod
        }

        /*
            "0" Off
            "1" Error messages
            "2" Warning, gives Error and Warning messages
            "3" Information, gives Error, Warning and Information messages
            "4" Verbose, gives Error, Warning, Information and Verbose messages
         */
        private static readonly string SWITCH_VALUE = ConfigurationSettings.AppSettings["LogUpToLevel"]==null?"4": ConfigurationSettings.AppSettings["LogUpToLevel"];
        private const string LOG_FOLDER = "RunningLogs";

        private static readonly string fileName;

        private static readonly string logIndentationMarker = "-->";
        private static string logIndentation = "";

        public static string Filename 
        {
            get
            {
                return fileName;
            }
        }

        private static readonly TraceSwitch traceSwitch = new TraceSwitch(AppDomain.CurrentDomain.FriendlyName,
                                                          "Simple Log.",
                                                          SWITCH_VALUE);
        static Log()
        {
            bool isFileNameOK = false;
            fileName = AppDomain.CurrentDomain.FriendlyName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
            var destFolder = Path.Combine(Environment.CurrentDirectory,
                                          LOG_FOLDER);
            try
            {
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can't create log file", ex);
            }

            try
            {
                //try to access file, if you can - it's fileName is valid.
                // ReSharper disable once UnusedVariable - this is how we check if file is valid for windows 7.
                var fileInfo = new System.IO.FileInfo(fileName);
                isFileNameOK = true;
            }
            catch (NotSupportedException ex)
            {
                //Thrown when running unit tests.
                Trace.WriteLine(ex);
            }

            if (!isFileNameOK)
            {
                fileName = "MaSIB_" + DateTime.Now.ToString("yyyyMMddHHmmss")+".txt";
            }

            fileName = Path.Combine(destFolder, fileName);
            TraceManager.InitLog(fileName);
        }

        public static void WriteLineIf(object value, TraceLevel level)
        {
            Trace.WriteLineIf((traceSwitch.Level.CompareTo(level) >= 0), "[" + DateTime.Now + "|"+String.Format("{0,-7}", level.ToString())+"]"+logIndentation + value);
        }

        public static void WriteFlowMarker(FlowMarker position,TraceLevel level)
        {
            StackTrace st = new StackTrace ();
            StackFrame sf = st.GetFrame (1);
            MethodBase method = sf.GetMethod();
            var fullName = string.Format("{0}.{1}({2})", method.ReflectedType.FullName, method.Name, string.Join(",", method.GetParameters().Select(o => string.Format("{0} {1}", o.ParameterType, o.Name)).ToArray()));
            WriteLineIf(position.ToString() + "::" + fullName, level);
            //WriteLineIf(position.ToString() + "::" + currentMethod.ReflectedType.Name + "::" + currentMethod.Name, TraceLevel.Info);
        }

        public static void WriteFlowMarker(FlowMarker position)
        {
            if (position == FlowMarker.ExitMethod)
            {

                if (logIndentation.Length>=logIndentationMarker.Length)
                {
                    logIndentation = logIndentation.Substring(logIndentationMarker.Length);
                }
           
            }
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            MethodBase method = sf.GetMethod();
            var fullName = string.Format("{0}.{1}({2})", method.ReflectedType.FullName, method.Name, string.Join(",", method.GetParameters().Select(o => string.Format("{0} {1}", o.ParameterType, o.Name)).ToArray()));
            WriteLineIf(position.ToString() + "::" + fullName, TraceLevel.Info);
            if (position == FlowMarker.EnterMethod)
            {
                logIndentation += logIndentationMarker;
            }
            //WriteLineIf(position.ToString() + "::" + currentMethod.ReflectedType.Name + "::" + currentMethod.Name, TraceLevel.Info);
        }
        public static void Close()
        {
            Trace.Close();
        }
    }

}


