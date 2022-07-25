using Mraznicka.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views.Pozicia
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListPage : ContentPage
	{
		ViewModels.Pozicia.ListViewModel _viewModel;

		public ListPage()
		{
			InitializeComponent();

			BindingContext = _viewModel = new ViewModels.Pozicia.ListViewModel();
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