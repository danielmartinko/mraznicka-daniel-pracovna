using Mraznicka.ViewModels;
using Xamarin.Forms;

namespace Mraznicka.Views.Zariadenie
{
	public partial class CreatePage : ContentPage
	{
		

		public CreatePage()
		{
			InitializeComponent();
			BindingContext = new ViewModels.Zariadenie.CreateViewModel(this);
		}
	}
}