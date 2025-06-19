using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApdateFilmUser.Convertes
{
    public class ScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Если value == true (элемент выбран), возвращаем 1.1, иначе 0.9
            return (value is bool isSelected && isSelected) ? 1.1 : 0.9;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Не требуется для одностороннего биндинга
        }
    }
}
