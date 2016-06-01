using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Gestionix.POS.GUI.Converters.POS
{
    public class MaximizedWindowBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (bool.Parse(value.ToString()))
                    return WindowState.Maximized;

                return WindowState.Normal;
            }
            catch { }

            return WindowState.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
