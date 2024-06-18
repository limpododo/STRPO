using System;
using System.Globalization;
using System.Windows.Data;

namespace CurrencyConverter.Converters;

public class CollapsedValueConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? "Visible" : "Collapsed";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToString().Equals("Visible");
    }
}