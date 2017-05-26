using LoggingHelper.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingHelper
{
    public static class LoggingHandlers
    {
        /// <summary>
        /// This method catches and handles all exceptions that aren't handled by try-catches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;

            Console.WriteLine("MyHandler caught : " + e.Message);
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);

            //Log the info to a log file
            string logs = "Logs\\Test_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            string line = string.Format("{0} User:{1}; AppName:{2}; ErrorMessage:{3}\n{{\n\tSource:{4}\n\tInnerException:{5}\n\tHResult:{6}\n\tStrackTrace:{7}\n}}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Environment.UserName, "Test", e.Message, e.Source, e.InnerException, e.HResult, e.StackTrace);
            //File.WriteAllText(logs, line);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logs, true))
            {
                file.WriteLine(line);
                file.WriteLine("ActionTrace:\n{");
                file.WriteLine(ActionTracer.ConvertToLogEntry() + "}");
            }
        }
    }
}
