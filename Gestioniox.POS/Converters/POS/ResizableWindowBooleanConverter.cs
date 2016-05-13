using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Gestionix.POS.GUI.Converters.POS
{
    public class ResizableWindowBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (bool.Parse(value.ToString()))
                    return ResizeMode.CanResize;

                return ResizeMode.NoResize;
            }
            catch { }

            return ResizeMode.CanResize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
