using System;
using System.Collections.Generic;
using System.Linq;
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
            StringBuilder ret = new StringBuilder(context);

            foreach (var str in _actions)
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
    }
}
