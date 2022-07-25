using Mraznicka.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Helpers
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;

            //AppResources.Culture = new System.Globalization.CultureInfo("pl-PL");

            return AppResources.ResourceManager.GetString(Text, AppResources.Culture);
            //return AppResources.ResourceManager.GetString(Text, System.Threading.Thread.CurrentThread.CurrentUICulture);
            //return AppResources.ResourceManager.GetString(Text);
        }
    }
}
