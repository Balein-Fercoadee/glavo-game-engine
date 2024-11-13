using System.Globalization;
using GameEngine.GameData;

namespace GameDatabaseEditor
{
    public class ObjectExistanceToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isVisible;
            if (value == null)
            {
                isVisible = false;
            }
            else
            {
                var selectedWord = (Word)value;
                isVisible = selectedWord.Id > 15;
            }
            return isVisible;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (bool)value ? new Room() : null;
        }
    }
}
