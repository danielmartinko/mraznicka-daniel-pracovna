using Mraznicka.ViewModels;
using Xamarin.Forms;

namespace Mraznicka.Views.Tovar
{
	public partial class CreatePage : ContentPage
	{
		

		public CreatePage()
		{
			InitializeComponent();
			BindingContext = new ViewModels.Tovar.CreateViewModel(this);
		}
	}
}