using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ValueConverters
{
    public class DateConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((DateTime)value).ToString("d.M.yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
