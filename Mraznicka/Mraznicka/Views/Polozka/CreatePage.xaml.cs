using Mraznicka.ViewModels;
using Xamarin.Forms;

namespace Mraznicka.Views.Polozka
{
	public partial class CreatePage : ContentPage
	{
		

		public CreatePage()
		{
			InitializeComponent();
			BindingContext = new ViewModels.Polozka.CreateViewModel();
		}
	}
}