using System.Globalization;
using GameEngine.GameData;

namespace GameDatabaseEditor.Converters
{
    /// <summary>Converts object presence, and optionally its ID, to a Boolean.</summary>
    public class ObjectExistanceToBoolConverter : IValueConverter
    {
        /// <summary>Checks whether an object exists and satisfies the optional ID threshold.</summary>
        /// <param name="value">The object to check; must be identifiable when a parameter is supplied.</param>
        /// <param name="targetType">The binding target type.</param>
        /// <param name="parameter">When non-null, requires the object's ID to be greater than 15.</param>
        /// <param name="culture">The binding culture.</param>
        /// <returns>False for null; otherwise true unless the optional ID check fails.</returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isVisible;
            if (value == null)
            {
                isVisible = false;
            }
            else
            {
                if (parameter == null)
                {
                    isVisible = true;
                }
                else
                {
                    var selectedWord = (IIdentifiable)value;
                    isVisible = selectedWord.Id > 15;
                }
            }
            return isVisible;
        }

        /// <summary>Creates a room when the value is true.</summary>
        /// <param name="value">The binding target value.</param>
        /// <param name="targetType">The binding source type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The binding culture.</param>
        /// <returns>A new room for true; otherwise null.</returns>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is true ? new Room() : null;
        }
    }
}
