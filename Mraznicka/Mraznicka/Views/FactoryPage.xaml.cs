using Mraznicka.ViewModels;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FactoryPage : ContentPage
	{
        public FactoryPage()
		{
			InitializeComponent();
            this.BindingContext = new FactoryViewModel(this);
        }
    }
}