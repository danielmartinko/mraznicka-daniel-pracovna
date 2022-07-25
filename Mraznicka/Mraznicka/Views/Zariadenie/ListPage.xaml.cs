using Mraznicka.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views.Zariadenie
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListPage : ContentPage
	{
		ViewModels.Zariadenie.ListViewModel _viewModel;

		public ListPage()
		{
			InitializeComponent();

			BindingContext = _viewModel = new ViewModels.Zariadenie.ListViewModel();
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