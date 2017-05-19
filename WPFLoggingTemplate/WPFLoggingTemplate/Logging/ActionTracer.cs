using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Purrfect.Logging
{
    public static class ActionTracer
    {
        private static int _maxEntries = 100;
        private static Queue<string> _actions = new Queue<string>();

        public static void PushAction(string context, params string[] argumentValues)
        {
            if (_actions.Count > _maxEntries)
            {
                _actions.Dequeue();
            }

            _actions.Enqueue(ActionToString(context, argumentValues));
        }

        private static string ActionToString(string context, string[] argumentValues)
        {
            StringBuilder ret = new StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +" "+ context);

            foreach (var str in argumentValues)
            {
                ret.AppendFormat(", {0}", str);
            }

            return ret.ToString();
        }

        public static string ConvertToLogEntry()
        {
            StringBuilder ret = new StringBuilder();

            foreach (var str in _actions)
            {
                ret.AppendLine(str);
            }

            return ret.ToString();
        }

        internal static void GetInstanceField(MethodBase methodBase, params string[] argumentValues)
        {
            //Not all methodBase's contain a return type since methodBases can be used to represent constructors which don't have a return type
            var normalMethod = methodBase as MethodInfo;
            var returntype = string.Empty;
            if (normalMethod != null)
            {
                returntype = normalMethod.ReturnType.ToString() + " ";
            }

            //Since returntype can be empty or it has a space at the end we don't need to add a space to the formatter
            //The membertype will tell us if the methodbase is e.g. method, class
            var name = string.Format("({0}) {1}'{2}'", methodBase.MemberType, returntype, methodBase.Name);

            //http://stackoverflow.com/questions/3303126/how-to-get-the-value-of-private-field-in-c
            //BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var values = methodBase.GetParameters().Select(x => x.Name).ToArray();

            if (argumentValues.Length == 0) //This means no parameter values were sent into the call
            {
                PushAction(name, values);
            }
            else
            {               
                if (values.Length > argumentValues.Length)
                {
                    throw new ArgumentException("There must be atleast the same number of parameter values as parameter names");
                }

                //Always assume there will be values.length <= argumentValues
                var pairs = new List<string>();
                for (int i = 0; i < values.Length; i++)
                {
                    pairs.Add(values[i] + "=" + argumentValues[i]);
                }

                //Sometimes we might want to include other parameter values or messages. These are included at the end
                if (values.Length < argumentValues.Length)
                {
                    for (int i = values.Length; i < argumentValues.Length; i++)
                    {
                        pairs.Add(argumentValues[i]);
                    }
                }

                PushAction(name, pairs.ToArray());
            }

            //methodBase.Name,
            //loop through values and add the exact value at the end
        }
    }
}
