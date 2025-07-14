using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using AIMuster.Config;

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

    public class ThemeToBoolConverter: IValueConverter
    {
        /// <summary>
        /// 正向：true → Dark，false → Light
        /// 反向：true → Light，false → Dark
        /// 参数：如果是 "Invert" 或 "False" → 反向模式
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var theme = (Theme)value;
                return theme.ToString().ToUpper() == parameter?.ToString()?.ToUpper();
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isChecked = (bool)value;
            if (Theme.Dark.ToString().ToUpper()== parameter?.ToString()?.ToUpper()&&isChecked)
            {
                return Theme.Dark;
            }
            if (Theme.Light.ToString().ToUpper() == parameter?.ToString()?.ToUpper() && isChecked)
            {
                return Theme.Light;
            }
            return Theme.Light;
        }
    }
}
