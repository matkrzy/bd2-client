using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BD_client.Converters
{
    public class ArrayStringToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<string> list = (List<String>) value;

            if (list.Count > 0)
                return String.Join(" ", list.ToArray());

            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            string rawList = (string) value;


            if (rawList.Length > 0)
            {
               return new List<string>(rawList.Split(' '));
            }

            return new List<string>();
        }
    }
}