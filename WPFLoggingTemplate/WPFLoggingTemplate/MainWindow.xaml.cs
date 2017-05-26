using LoggingHelper.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            string line = string.Format("{0} User:{1}; AppName:{2}; ErrorMessage:{3}\n{{\n\tSource:{4}\n\tInnerException:{5}\n\tHResult:{6}\n\tStrackTrace:{7}\n}}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Environment.UserName, "Test", e.Message, e.Source, e.InnerException, e.HResult, e.StackTrace);
            //File.WriteAllText(logs, line);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logs, true))
            {
                file.WriteLine(line);
                file.WriteLine("ActionTrace:\n{");
                file.WriteLine(ActionTracer.ConvertToLogEntry() + "}");
            }
        }

        int count = 0;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine(e.OriginalSource);
            //Console.WriteLine(this.btn_temp.Content);
            //Console.WriteLine(this.btn_temp.Name);
            //Console.WriteLine(e.RoutedEvent);
            ActionTracer.PushAction(this.btn_temp.Name, e.OriginalSource.ToString(), e.RoutedEvent.ToString());

            ActionTracer.PushMethodtoQueue(MethodBase.GetCurrentMethod());

            var testclass = new TestClass();

            var temp = test(9);

            if (count == 4)
            {
                throw new Exception("test");
            }
            count++;

            
        }
        //www.sourcetreeapp.com

        private static int test(int i)
        {
            //http://stackoverflow.com/questions/44153/can-you-use-reflection-to-find-the-name-of-the-currently-executing-method
            ActionTracer.PushMethodtoQueue(MethodBase.GetCurrentMethod(), i.ToString(), "Hello");
            return i;
        }
    }
}
