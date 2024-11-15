using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseEditor.Converters
{
    public class ItemListViewConverter : IMultiValueConverter
    {
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

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
