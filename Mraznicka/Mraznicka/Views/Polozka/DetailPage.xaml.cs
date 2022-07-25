using Mraznicka.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views.Polozka
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailPage : ContentPage
	{
		private ViewModels.Polozka.DetailViewModel ctx { get; set; }
		public DetailPage()
		{
			InitializeComponent();
			BindingContext = this.ctx = new ViewModels.Polozka.DetailViewModel(this);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		
		}
	}
}