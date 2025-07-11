using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace AIMuster.Converter
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 正向：true → Visible，false → Collapsed
        /// 反向：true → Collapsed，false → Visible
        /// 参数：如果是 "Invert" 或 "False" → 反向模式
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = value is bool b && b;
            bool isInverted = parameter?.ToString()?.ToLower() is "invert" or "false";

            if (isInverted)
                boolValue = !boolValue;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

}
