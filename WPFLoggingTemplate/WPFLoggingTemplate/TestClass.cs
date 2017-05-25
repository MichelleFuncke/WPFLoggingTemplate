using Purrfect.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WPFLoggingTemplate
{
    class TestClass
    {
        public TestClass()
        {
            //This is where we contruct the class
            ActionTracer.GetInstanceField(MethodBase.GetCurrentMethod());
        }
    }
}
