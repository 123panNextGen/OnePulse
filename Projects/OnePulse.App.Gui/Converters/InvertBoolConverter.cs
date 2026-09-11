using Microsoft.UI.Xaml.Data;
using System;

namespace OnePulse.App.Gui.Converters
{
    public partial class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
                return !boolValue;
            return false; // 或其他默认值
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
                return !boolValue;
            return false;
        }
    }
}
