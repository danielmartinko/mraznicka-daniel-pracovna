using Mraznicka.Services;
using Mraznicka.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views.Vlozenie
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AllInOnePage : ContentPage
	{
		public IDataStore<Models.Miestnost> MiestnostDataStore => DependencyService.Get<IDataStore<Models.Miestnost>>();
		public IDataStore<Models.Zariadenie> ZariadenieDataStore => DependencyService.Get<IDataStore<Models.Zariadenie>>();
		public IDataStore<Models.Pozicia> PoziciaDataStore => DependencyService.Get<IDataStore<Models.Pozicia>>();
		public IDataStore<Models.Tovar> TovarDataStore => DependencyService.Get<IDataStore<Models.Tovar>>();

		private ViewModels.Vlozenie.AllInOnePageViewModel ctx { get; set; }


		public AllInOnePage()
		{
			InitializeComponent();
			this.BindingContext = this.ctx = new ViewModels.Vlozenie.AllInOnePageViewModel(this);
			
			Populate();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			this.ctx.OnAppearing();
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