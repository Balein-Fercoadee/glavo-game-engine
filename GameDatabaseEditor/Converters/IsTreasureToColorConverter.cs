using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseEditor.Converters
{
    /// <summary>Converts a treasure flag to a brush.</summary>
    public class IsTreasureToColorConverter : IValueConverter
    {
        /// <summary>Chooses a green brush for a treasure.</summary>
        /// <param name="value">A Boolean treasure flag, or null.</param>
        /// <param name="targetType">The binding target type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The binding culture.</param>
        /// <returns>Green for true; otherwise the default brush.</returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Brush color = Brush.Default;
            if (value != null)
            {
                color = (bool)value ? Brush.Green : Brush.Default; 
            }
            return color;
        }

        /// <summary>Returns null; reverse conversion is not supported.</summary>
        /// <param name="value">The binding target value.</param>
        /// <param name="targetType">The binding source type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The binding culture.</param>
        /// <returns>Always null.</returns>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
