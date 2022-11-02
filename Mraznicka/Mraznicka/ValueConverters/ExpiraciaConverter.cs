using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ValueConverters
{
    public class ExpiraciaConverter : IValueConverter
    {
        public IDataStore<Models.Tovar> DataStoreTovar => DependencyService.Get<IDataStore<Models.Tovar>>();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Color.Transparent;

            var tovar = DataStoreTovar.GetItem(((Mraznicka.Models.Polozka)value).Tovar);
            int expiracia10percent = tovar.Expiracia * 10 / 100;
            int expiracia20percent = tovar.Expiracia * 20 / 100;

            // Percenta 10 a 20
            if (DateTime.Now.AddDays(-1) > ((Mraznicka.Models.Polozka)value).Expiracia)
            {
                return Color.Black;
            }
            if (DateTime.Now.AddDays(expiracia10percent) > ((Mraznicka.Models.Polozka)value).Expiracia)
            {
                return Color.Red;
            }
            if (DateTime.Now.AddDays(expiracia20percent) > ((Mraznicka.Models.Polozka)value).Expiracia)
            {
                return Color.DarkOrange;
            }
            /*
            // Dni 7 a 14
            if (DateTime.Now.AddDays(7) > ((Mraznicka.Models.Polozka)value).Expiracia)
            {
                return Color.DarkRed;
            }
            if (DateTime.Now.AddDays(14) > ((Mraznicka.Models.Polozka)value).Expiracia)
            {
                return Color.Red;
            }
            */

            return Color.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
