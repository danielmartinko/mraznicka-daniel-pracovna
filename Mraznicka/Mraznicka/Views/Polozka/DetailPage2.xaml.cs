using Mraznicka.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views.Polozka
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailPage2 : ContentPage
	{
		private ViewModels.Polozka.DetailViewModel2 ctx { get; set; }
		public DetailPage2()
		{
			InitializeComponent();
			BindingContext = this.ctx = new ViewModels.Polozka.DetailViewModel2(this);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		
		}
	}
}