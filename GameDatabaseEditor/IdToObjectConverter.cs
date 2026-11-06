using GameEngine.GameData;
using GameDatabaseEditor.Utilities;
using System.Globalization;


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
            if (value is not int objectId || objectId <= -2)
            {
                return null;
            }

            if (parameter is Binding binding)
            {
                parameter = BindingUtilities.GetPropertyValue(binding.Source, binding.Path);
            }

            if (parameter is not IEnumerable<IIdentifiable> identifiables)
            {
                return null;
            }

            return identifiables.FirstOrDefault(item => item.Id == objectId);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is IIdentifiable identifiable ? identifiable.Id : null;
        }
    }
}
