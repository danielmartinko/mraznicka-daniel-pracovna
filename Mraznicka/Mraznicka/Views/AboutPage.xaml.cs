using Mraznicka.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
		public AboutPage()
		{
			InitializeComponent();
			BindingContext = new AboutViewModel();
		}
	}
}