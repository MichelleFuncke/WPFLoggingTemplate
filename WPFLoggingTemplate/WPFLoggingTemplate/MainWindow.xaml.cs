using Purrfect.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFLoggingTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //So that I handle exceptions
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

            InitializeComponent();
        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;

            Console.WriteLine("MyHandler caught : " + e.Message);
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);

            //Log the info to a log file
            string logs = "Logs\\Test_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            string line = string.Format("{0};{1};{2} {3}\n(\n\tSource:{4}\n\tInnerException:{5}\n\tHResult:{6}\n\tStrackTrace:{7}\n)", DateTime.Now, Environment.UserName, "Test", e.Message, e.Source, e.InnerException, e.HResult, e.StackTrace);
            //File.WriteAllText(logs, line);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logs, true))
            {
                //file.WriteLine(line);
                file.WriteLine(ActionTracer.ConvertToLogEntry());
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ActionTracer.PushAction("button", sender.ToString(), e.ToString());
            throw new Exception("test");
        }
    }
}
