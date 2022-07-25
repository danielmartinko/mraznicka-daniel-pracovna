using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ValueConverters
{
    public class ZariadenieConverter : IValueConverter
    {

        public IDataStore<Models.Zariadenie> DataStore => DependencyService.Get<IDataStore<Models.Zariadenie>>();


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DataStore.GetItem((int)value).Nazov;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
