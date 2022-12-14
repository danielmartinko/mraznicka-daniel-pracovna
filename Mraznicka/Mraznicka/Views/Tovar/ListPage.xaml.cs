using Mraznicka.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views.Tovar
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListPage : ContentPage
	{
		ViewModels.Tovar.ListViewModel _viewModel;

		public ListPage()
		{
			InitializeComponent();

			BindingContext = _viewModel = new ViewModels.Tovar.ListViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_viewModel.OnAppearing();
		}

		protected override bool OnBackButtonPressed()
		{
			Shell.Current.GoToAsync("MainPage");
			return true;
		}
	}
}