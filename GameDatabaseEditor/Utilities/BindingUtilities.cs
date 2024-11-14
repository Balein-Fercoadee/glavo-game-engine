using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseEditor.Utilities
{
    public static class BindingUtilities
    {
        public static object GetPropertyValue(object source, string propertyName)
        {
            if (propertyName.Contains("."))
            {
                var splitIndex = propertyName.IndexOf('.');
                var parent = propertyName.Substring(0, splitIndex);
                var child = propertyName.Substring(splitIndex + 1);
                var obj = source?.GetType().GetProperty(parent)?.GetValue(source, null);
                return GetPropertyValue(obj, child);
            }
            return source?.GetType().GetProperty(propertyName)?.GetValue(source, null);
        }
    }
}
