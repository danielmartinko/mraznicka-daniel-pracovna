using Mraznicka.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka
{
	public partial class AppShell : Xamarin.Forms.Shell
	{

		/*
		
		route		The route hierarchy will be searched for the specified route, upwards from the current position. The matching page will be pushed to the navigation stack.
		/route		The route hierarchy will be searched from the specified route, downwards from the current position. The matching page will be pushed to the navigation stack.
		//route		The route hierarchy will be searched for the specified route, upwards from the current position. The matching page will replace the navigation stack.
		///route	The route hierarchy will be searched for the specified route, downwards from the current position. The matching page will replace the navigation stack.
		
		 */

		public AppShell()
		{
			InitializeComponent();

			
			Routing.RegisterRoute("MainPage", typeof(MainPage));

			Routing.RegisterRoute("MiestnostListPage", typeof(Views.Miestnost.ListPage));
			Routing.RegisterRoute("MiestnostDetailPage", typeof(Views.Miestnost.DetailPage));
			Routing.RegisterRoute("MiestnostCreatePage", typeof(Views.Miestnost.CreatePage));

			Routing.RegisterRoute("PoziciaListPage", typeof(Views.Pozicia.ListPage));
			Routing.RegisterRoute("PoziciaDetailPage", typeof(Views.Pozicia.DetailPage));
			Routing.RegisterRoute("PoziciaCreatePage", typeof(Views.Pozicia.CreatePage));

			Routing.RegisterRoute("TovarListPage", typeof(Views.Tovar.ListPage));
			Routing.RegisterRoute("TovarDetailPage", typeof(Views.Tovar.DetailPage));
			Routing.RegisterRoute("TovarCreatePage", typeof(Views.Tovar.CreatePage));

			Routing.RegisterRoute("ZariadenieListPage", typeof(Views.Zariadenie.ListPage));
			Routing.RegisterRoute("ZariadenieDetailPage", typeof(Views.Zariadenie.DetailPage));
			Routing.RegisterRoute("ZariadenieCreatePage", typeof(Views.Zariadenie.CreatePage));

			Routing.RegisterRoute("PolozkaListPage", typeof(Views.Polozka.ListPage));
			Routing.RegisterRoute("PolozkaDetailPage", typeof(Views.Polozka.DetailPage));
			Routing.RegisterRoute("PolozkaCreatePage", typeof(Views.Polozka.CreatePage));


			Routing.RegisterRoute("PushPage", typeof(Views.PushPage));
			Routing.RegisterRoute("PullPage", typeof(Views.PullPage));
			Routing.RegisterRoute("PreviewPage", typeof(Views.PreviewPage));


			Routing.RegisterRoute("VlozenieEanCodePage", typeof(Views.Vlozenie.EANCodePage));
			Routing.RegisterRoute("VyberEanCodePage", typeof(Views.Vyber.EANCodePage));

			Routing.RegisterRoute("VlozenieTagPage", typeof(Views.Vlozenie.TagPage));
			Routing.RegisterRoute("VyberTagPage", typeof(Views.Vyber.TagPage));

			Routing.RegisterRoute("VlozenieManualPage", typeof(Views.Vlozenie.ManualPage));
			Routing.RegisterRoute("VyberManualPage", typeof(Views.Vyber.ManualPage));

			Routing.RegisterRoute("ExpressPush", typeof(Views.Vlozenie.ExpressPage));
			Routing.RegisterRoute("ExpressPull", typeof(Views.Vyber.ExpressPage));

			Routing.RegisterRoute("CompareEanPage", typeof(Views.Vyber.CompareEANPage));
			Routing.RegisterRoute("CompareTagPage", typeof(Views.Vyber.CompareTagPage));
			Routing.RegisterRoute("CompareManualPage", typeof(Views.Vyber.CompareManualPage));

			Routing.RegisterRoute("VlozenieAllInOnePage", typeof(Views.Vlozenie.AllInOnePage));
			Routing.RegisterRoute("VlozenieAllInOnePageTag", typeof(Views.Vlozenie.AllInOnePageTag));
			Routing.RegisterRoute("VlozenieAllInOnePageEan", typeof(Views.Vlozenie.AllInOnePageEan));
		}

		private async void OnMenuItemClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync("//LoginPage");
		}
	}
}