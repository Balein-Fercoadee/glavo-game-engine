using System.Globalization;
using GameEngine.GameData;

namespace GameDatabaseEditor
{
    public class ObjectExistanceToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value != null ? true : false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (bool)value ? new Room() : null;
        }
    }
}
