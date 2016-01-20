using System.Collections.Generic;
using System.Reflection;

namespace Heartbeat.Models
{
    static class ListHelper
    {
        public static void SetPropertyFromList<T>(List<string> items, ref List<T> channels, string property)
        {
            for (int index = 0; index < channels.Count; ++index)
                TrySetProperty(channels[index], property, items[index]);
        }

        private static void TrySetProperty(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
                prop.SetValue(obj, value, null);
        }
    }
}
