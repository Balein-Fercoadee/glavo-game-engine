using GameEngine.GameData;
using System.Globalization;

namespace GameDatabaseEditor
{
    /// <summary>
    /// Converts an ID (values[0]) and a collection of <see cref="Item"/> (values[1]) to "Id - Name" text.
    /// </summary>
    public class IdToDisplayTextConverter : IMultiValueConverter
    {
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

        public object[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
