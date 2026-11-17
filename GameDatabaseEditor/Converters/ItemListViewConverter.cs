using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseEditor.Converters
{
    /// <summary>Formats item values for display in a list.</summary>
    public class ItemListViewConverter : IMultiValueConverter
    {
        /// <summary>Formats the first two values and marks treasure with an asterisk.</summary>
        /// <param name="values">At least four values; the third must be a Boolean treasure flag. The fourth is unused.</param>
        /// <param name="targetType">The binding target type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The binding culture.</param>
        /// <returns>Formatted text, or empty text if fewer than four values are supplied.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string output = string.Empty;
            if (values != null && values.Length > 3)
            {
                string isTreasure = (bool)values[2] == true ? "*" : "";
                output = string.Format("{0} - {1} {2}", values[0].ToString(), values[1].ToString(), isTreasure);
            }
            return output;
        }

        /// <summary>Returns no source values; reverse conversion is not supported.</summary>
        /// <param name="value">The binding target value.</param>
        /// <param name="targetTypes">The binding source types.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The binding culture.</param>
        /// <returns>An empty array.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return Array.Empty<object>();
        }
    }
}
