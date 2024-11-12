using GameEngine.GameData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseEditor
{
    public class IdToObjectConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">This should be the collection of objects to do an ID search.</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Object gameObject = null;
            int objectId = (int)value;
            if (objectId > -1)
            {
                if(parameter is Binding binding)
                {
                    parameter = GetPropertyValue(binding.Source, binding.Path);
                }

                IEnumerable<IIdentifiable> identifiables = (IEnumerable<IIdentifiable>)parameter;
                var identifiable = identifiables.Where(i => i.Id == objectId).FirstOrDefault();
                gameObject = identifiable;
            }

            return gameObject;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int objectId = ((IIdentifiable)value).Id;
            return objectId;
        }

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
