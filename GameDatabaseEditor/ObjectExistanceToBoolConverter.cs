using System.Globalization;
using GameEngine.GameData;

namespace GameDatabaseEditor
{
    public class ObjectExistanceToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isVisible = value != null ? true : false;
            return isVisible;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (bool)value ? new Room() : null;
        }
    }
}
