using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseEditor.Converters
{
    /// <summary>
    /// Highlights IDs at or below a threshold with a red brush.
    /// </summary>
    public class IdToColorConverter : IValueConverter
    {
        /// <summary>Converts an integer ID to a red or transparent brush.</summary>
        /// <param name="value">The integer ID, or null.</param>
        /// <param name="targetType">The binding target type.</param>
        /// <param name="parameter">A non-null value changes the threshold from -1 to 0.</param>
        /// <param name="culture">The binding culture.</param>
        /// <returns>Red for null or an ID at or below the threshold; otherwise transparent.</returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Brush brushColor = Brush.Red;
            if (value != null)
            {
                int numberToCompare;
                if(parameter == null)
                {
                    numberToCompare = -1;
                }
                else
                {
                    numberToCompare = 0;
                }

                brushColor = (int)value > numberToCompare ? Brush.Transparent : Brush.Red;
            }
            return brushColor;
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
