using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace AIMuster.Converter
{
    public class MultiSelectCommandParameterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new
            {
                SelectedItem = values[0],  // ComboBox.SelectedItem
                CurrentDataItem = values[1],   // 当前 ItemTemplate 的数据对象
                ComboBox = values[2]
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
