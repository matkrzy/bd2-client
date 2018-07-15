using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BD_client.Dto;

namespace BD_client.Converters
{
    class CategoriestToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<Category> list = (List<Category>) value;
            List<string> names = new List<string>();

            if (list == null || list.Count == 0)
            {
                return "-";
            }

            foreach (Category category in list)
            {
                names.Add(category.Name);
            }

            return String.Join(" ", names.ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}