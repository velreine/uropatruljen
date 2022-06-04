using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartUro.Converters;

/// <summary>
/// This seems so idiotic but this prevents us from having to keep 2 properties for controlling 1 state.
/// Basically: Our ViewModels will not need "IsBusy" and "IsNotBusy", but can simply rely on "IsBusy".
/// </summary>
public class InverseBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(bool))
        {
            throw new Exception("The InverseBoolConverter can only be applied to objects of type bool(ean).");
        }
        
        return !(bool)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType != typeof(bool))
        {
            throw new Exception("The InverseBoolConverter can only be applied to objects of type bool(ean).");
        }
        
        return !(bool)value;
    }
}