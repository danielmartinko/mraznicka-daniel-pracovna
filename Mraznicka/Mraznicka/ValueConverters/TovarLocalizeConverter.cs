using Mraznicka.Models;
using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ValueConverters
{
    public class TovarLocalizeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (((Tovar)value).Id)
            {
                case 1:
                    return Resources.AppResources.bravcove;
                case 2:
                    return Resources.AppResources.kuracie;
                case 3:
                    return Resources.AppResources.hovadzie;
                case 4:
                    return Resources.AppResources.hydina;
                case 5:
                    return Resources.AppResources.zverina;
                case 6:
                    return Resources.AppResources.masovevyrobky;
                case 7:
                    return Resources.AppResources.hotovejedlo;
                case 8:
                    return Resources.AppResources.polotovar;
                case 9:
                    return Resources.AppResources.zelenina;
                case 10:
                    return Resources.AppResources.ovocie;
                case 11:
                    return Resources.AppResources.bylinky;
                case 12:
                    return Resources.AppResources.pecivo;
                case 13:
                    return Resources.AppResources.cukrovinky;
                case 14:
                    return Resources.AppResources.mliecnevyrobky;
                case 15:
                    return Resources.AppResources.ryby;
                case 16:
                    return Resources.AppResources.ine;
                default:
                    return ((Tovar)value).Nazov;

            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
