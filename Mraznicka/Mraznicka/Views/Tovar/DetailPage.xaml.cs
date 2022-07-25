using Mraznicka.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views.Tovar
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailPage : ContentPage
	{
		public DetailPage()
		{
			InitializeComponent();
			BindingContext = new ViewModels.Tovar.DetailViewModel(this);
		}
	}
}