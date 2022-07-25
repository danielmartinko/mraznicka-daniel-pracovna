using Mraznicka.ViewModels;
using Xamarin.Forms;

namespace Mraznicka.Views.Miestnost
{
	public partial class CreatePage : ContentPage
	{
		

		public CreatePage()
		{
			InitializeComponent();
			BindingContext = new ViewModels.Miestnost.CreateViewModel();
		}
	}
}