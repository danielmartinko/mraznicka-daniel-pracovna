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



/*
 
 ContentProperty("Text")]
public class TranslateExtension : IMarkupExtension
{
    private readonly CultureInfo _ci;

    static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(
        () => new ResourceManager(typeof(AppResources).FullName, typeof(TranslateExtension).GetTypeInfo().Assembly));

    public string Text { get; set; }

    public TranslateExtension()
    {
        if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
        {
            _ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
        }
    }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Text == null)
            return string.Empty;

        return ResMgr.Value.GetString(Text, _ci) ?? Text;
    }
}
 
 
 */