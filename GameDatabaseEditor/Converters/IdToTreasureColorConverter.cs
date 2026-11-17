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
        public object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is int id && values[1] is IEnumerable<Item> items
                && items.FirstOrDefault(i => i.Id == id)?.IsTreasure == true)
            {
                return Colors.Gold;
            }

            return BindableProperty.UnsetValue;
        }

        public object[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
