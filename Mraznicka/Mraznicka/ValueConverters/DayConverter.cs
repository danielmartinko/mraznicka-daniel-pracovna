using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ValueConverters
{
    public class DayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime dtExpiracia = (DateTime)value;
            TimeSpan dni = dtExpiracia - DateTime.Now;
            switch (dni.Days)
            {
                case -1:
                    return Resources.AppResources.po_expiracii + -dni.Days + Resources.AppResources.den;
                case -2:
                case -3:
                case -4:
                    return Resources.AppResources.po_expiracii + -dni.Days + Resources.AppResources.dni;
                case 1:
                    return Resources.AppResources.do_expiracie + dni.Days + Resources.AppResources.den;
                case 2:
                case 3:
                case 4:
                    return Resources.AppResources.do_expiracie + dni.Days + Resources.AppResources.dni;
            }
            if ( dni.Days < 0)
                return Resources.AppResources.po_expiracii + -dni.Days + Resources.AppResources.dní;
            else
                return Resources.AppResources.do_expiracie + dni.Days + Resources.AppResources.dní;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
