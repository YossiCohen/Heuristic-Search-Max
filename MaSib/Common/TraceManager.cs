using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class TraceManager
    {
        public static void InitLog(string logFileName)
        {
            TextWriterTraceListener consoleTraceListener = new TextWriterTraceListener(Console.Out);
            Trace.Listeners.Add(consoleTraceListener);

            TextWriterTraceListener textWriterTraceListener = new TextWriterTraceListener(logFileName);
            Trace.Listeners.Add(textWriterTraceListener);

            Trace.AutoFlush = true;

            Log.WriteLineIf("Log started.", TraceLevel.Off);
        }
    }
}