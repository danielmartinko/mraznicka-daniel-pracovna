using Mraznicka.ViewModels;
using Xamarin.Forms;

namespace Mraznicka.Views.Pozicia
{
	public partial class CreatePage : ContentPage
	{
		

		public CreatePage()
		{
			InitializeComponent();
			BindingContext = new ViewModels.Pozicia.CreateViewModel(this);
		}
	}
}