using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseEditor
{
    public class IdToColorConverter : IValueConverter
    {
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

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
