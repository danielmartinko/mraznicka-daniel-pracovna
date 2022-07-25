using Mraznicka.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views.Miestnost
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailPage : ContentPage
	{
		public DetailPage()
		{
			InitializeComponent();
			BindingContext = new ViewModels.Miestnost.DetailViewModel();
		}
	}
}