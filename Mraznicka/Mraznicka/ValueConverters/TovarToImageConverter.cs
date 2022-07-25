using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ValueConverters
{
    public class TovarToImageConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((int)value)
            {
                case 1:
                    return "ikony_bravcove";
                case 2:
                    return "ikony_kuracie";
                case 3:
                    return "ikony_hovadzie";
                case 4:
                    return "ikony_hydina";
                case 5:
                    return "ikony_zverina";
                case 6:
                    return "ikony_masovevyrobky";
                case 7:
                    return "ikony_hotovejedlo";
                case 8:
                    return "ikony_polotovar";
                case 9:
                    return "ikony_zelenina";
                case 10:
                    return "ikony_ovocie";
                case 11:
                    return "ikony_bylinky";
                case 12:
                    return "ikony_pecivo";
                case 13:
                    return "ikony_cukrovinky";
                case 14:
                    return "ikony_mliecnevyrobky";
                case 15:
                    return "ikony_ryby";
                default:
                    return "ikony_hocico.png";

            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
