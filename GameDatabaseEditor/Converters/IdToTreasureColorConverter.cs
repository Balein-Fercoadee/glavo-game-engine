using GameEngine.GameData;
using System.Globalization;

namespace GameDatabaseEditor.Converters
{
    /// <summary>
    /// Converts an Item ID (values[0]) and a collection of <see cref="Item"/> (values[1]) to Gold for treasures.
    /// Returns <see cref="BindableProperty.UnsetValue"/> for non-treasures so the default text color applies.
    /// </summary>
    public class IdToTreasureColorConverter : IMultiValueConverter
    {
        /// <summary>Looks up an item and chooses its text color.</summary>
        /// <param name="values">An integer item ID followed by a collection of items.</param>
        /// <param name="targetType">The binding target type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The binding culture.</param>
        /// <returns>Gold for a matching treasure; otherwise <see cref="BindableProperty.UnsetValue"/>.</returns>
        public object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is int id && values[1] is IEnumerable<Item> items
                && items.FirstOrDefault(i => i.Id == id)?.IsTreasure == true)
            {
                return Colors.Gold;
            }

            return BindableProperty.UnsetValue;
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
