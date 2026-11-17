using GameEngine.GameData;
using GameDatabaseEditor.Utilities;
using System.Globalization;


namespace GameDatabaseEditor.Converters
{
    /// <summary>Converts between an ID and an identifiable object in a collection.</summary>
    public class IdToObjectConverter : IValueConverter
    {
        /// <summary>Finds the first object with the specified ID.</summary>
        /// <param name="value">The integer ID to find; IDs below -1 are rejected.</param>
        /// <param name="targetType">The binding target type.</param>
        /// <param name="parameter">A collection of identifiable objects, or a binding to one.</param>
        /// <param name="culture">The binding culture.</param>
        /// <returns>The matching object, or null if the inputs are invalid or no match exists.</returns>
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

        /// <summary>Extracts the ID from an identifiable object.</summary>
        /// <param name="value">The object to convert.</param>
        /// <param name="targetType">The binding source type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The binding culture.</param>
        /// <returns>The object's ID, or null if it is not identifiable.</returns>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is IIdentifiable identifiable ? identifiable.Id : null;
        }
    }
}
