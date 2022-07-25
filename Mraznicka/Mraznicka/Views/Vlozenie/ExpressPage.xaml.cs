using Mraznicka.Services;
using Mraznicka.ViewModels.Vlozenie;
using Plugin.NFC;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

/*
 
 https://github.com/franckbour/Plugin.NFC
 
 */

namespace Mraznicka.Views.Vlozenie
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExpressPage : ContentPage
	{
		public IDataStore<Models.Miestnost> MiestnostDataStore => DependencyService.Get<IDataStore<Models.Miestnost>>();
		public IDataStore<Models.Zariadenie> ZariadenieDataStore => DependencyService.Get<IDataStore<Models.Zariadenie>>();
		public IDataStore<Models.Pozicia> PoziciaDataStore => DependencyService.Get<IDataStore<Models.Pozicia>>();
		public IDataStore<Models.Tovar> TovarDataStore => DependencyService.Get<IDataStore<Models.Tovar>>();

		private ExpressPageViewModel ctx { get; set; }

		public ExpressPage()
		{
			InitializeComponent();
			this.BindingContext = this.ctx = new ViewModels.Vlozenie.ExpressPageViewModel(this);

			Populate();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ctx.OnAppearing();
			_scanView.IsScanning = true;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			_scanView.IsScanning = false;
		}

		public void Handle_OnScanResult(Result result)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				ctx.Item.TagID = result.Text;
				//await DisplayAlert("Uspesne naskenovany EAN", result.Text, "OK");
			});
		}

		public void Populate()
		{
			ctx.Miestnosti.Clear();
			ctx.Zariadenia.Clear();
			ctx.Pozicie.Clear();
			ctx.Tovary.Clear();

			foreach (var item in MiestnostDataStore.GetItems(true))
			{
				ctx.Miestnosti.Add(item);
			}

			foreach (var item in ZariadenieDataStore.GetItems(true))
			{
				ctx.Zariadenia.Add(item);
			}

			foreach (var item in PoziciaDataStore.GetItems(true))
			{
				ctx.Pozicie.Add(item);
			}

			foreach (var item in TovarDataStore.GetItems(true))
			{
				ctx.Tovary.Add(item);
			}

			var pp = ((App)Application.Current).PoslednePouzite;

			int a = 0;
			int b = 0;
			int c = 0;
			foreach (var item in ctx.Tovary)
			{
				if (pp == null)
				{
					pckTovar.SelectedIndex = a;
					break;
				}


				if (item.Id == pp.Tovar)
					pckTovar.SelectedIndex = a;
				a++;

			}


			foreach (var item in ctx.Pozicie)
			{
				if (pp == null)
				{
					pckPozicia.SelectedIndex = b;
					break;
				}

				if (item.Id == pp.Pozicia)
					pckPozicia.SelectedIndex = b;
				b++;
			}

			foreach (var item in ctx.Zariadenia)
			{
				if (pp == null)
				{
					pckZariadenie.SelectedIndex = c;
					break;
				}

				if (item.Id == pp.Zariadenie)
					pckZariadenie.SelectedIndex = c;
				c++;
			}

			if (pckTovar.SelectedIndex == -1)
				pckTovar.SelectedIndex = 0;

			if (pckPozicia.SelectedIndex == -1)
				pckPozicia.SelectedIndex = 0;

			if (pckZariadenie.SelectedIndex == -1)
				pckZariadenie.SelectedIndex = 0;
		}

	}
}