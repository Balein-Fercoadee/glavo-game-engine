using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseEditor
{
    public class RoomDescriptionStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string description = string.Empty;
            if (values != null && values[0] != null && values[1] != null)
            {
                int roomId = (int)values[0];
                string roomName = (string)values[1];
                description = string.Format("{0;000} - {1}", roomId, roomName);
            }

            return description;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
