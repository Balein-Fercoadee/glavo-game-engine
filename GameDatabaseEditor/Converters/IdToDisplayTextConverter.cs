using GameEngine.GameData;
using System.Globalization;

namespace GameDatabaseEditor.Converters
{
    /// <summary>
    /// Formats an ID and the name of its matching identifiable object for display.
    /// </summary>
    public class IdToDisplayTextConverter : IMultiValueConverter
    {
        /// <summary>Formats the matching object's ID and name.</summary>
        /// <param name="values">An integer ID followed by a collection of identifiable objects.</param>
        /// <param name="targetType">The binding target type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The binding culture.</param>
        /// <returns>Formatted ID and name, the ID alone if not found, or empty text for invalid inputs.</returns>
        public object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] is not int id || values[1] is not IEnumerable<IIdentifiable> source)
            {
                return string.Empty;
            }

            var item = source.FirstOrDefault(i => i.Id == id);
            if (item is null)
            {
                return id.ToString();
            }

            var name = item.GetType().GetProperty("Name")?.GetValue(item) as string;
            return $"{id,3} - {name}";
        }

        /// <summary>Reverse conversion is not supported.</summary>
        /// <param name="value">The binding target value.</param>
        /// <param name="targetTypes">The binding source types.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The binding culture.</param>
        /// <returns>No value is returned.</returns>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        public object[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
