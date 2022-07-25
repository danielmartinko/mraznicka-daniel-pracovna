using Mraznicka.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views.Polozka
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListPage : ContentPage
	{
		ViewModels.Polozka.ListViewModel _viewModel;

		public ListPage()
		{
			InitializeComponent();

			BindingContext = _viewModel = new ViewModels.Polozka.ListViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.OnAppearing();
		}
	}
}