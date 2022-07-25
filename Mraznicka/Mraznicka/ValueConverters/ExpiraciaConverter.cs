using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ValueConverters
{
    public class ExpiraciaConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Color.Transparent;


            if (DateTime.Now.AddDays(7) > ((Mraznicka.Models.Polozka)value).Expiracia)
            {
                return Color.DarkRed;
            }

            if (DateTime.Now.AddDays(14) > ((Mraznicka.Models.Polozka)value).Expiracia)
            {
                return Color.Red;
            }

            

            return Color.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
