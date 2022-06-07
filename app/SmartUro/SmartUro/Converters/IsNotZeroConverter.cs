using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartUro.Converters
{
    public class IsNotZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is not 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not necessary to implement.
            throw new NotImplementedException();
        }
    }
}