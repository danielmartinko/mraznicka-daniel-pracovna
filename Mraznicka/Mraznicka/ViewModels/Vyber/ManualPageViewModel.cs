using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Mraznicka.DMToast;

namespace Mraznicka.ViewModels.Vyber
{
	public class ManualPageViewModel : BaseViewModel
	{
		public Command<Models.Polozka> ItemTapped { get; }

		public ObservableCollection<Models.Polozka> Items { get; }

		public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();
		public IDataStore<Models.Zariadenie> ZariadenieDataStore => DependencyService.Get<IDataStore<Models.Zariadenie>>();

		public ObservableCollection<Models.Zariadenie> Zariadenia { get; } = new ObservableCollection<Models.Zariadenie>();

		public ContentPage contentPage { get; set; }

		public int SelectedZariadenie { get; set; }

		public Command LoadItemsCommand { get; }

		private string filter;

		public string Filter
		{
			get
			{
				return filter;
			}
			set
			{
				filter = value;
				ExecuteLoadItemsCommand();
			}
		}

		public ManualPageViewModel(ContentPage page)
		{
			contentPage = page;
			Items = new ObservableCollection<Models.Polozka>();
			ItemTapped = new Command<Models.Polozka>(OnItemSelected);
			LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
			Zariadenia.Clear();
			Zariadenia.Add(new Models.Zariadenie() { Id = -1, Nazov = Resources.AppResources.vyberte_zariadenia_vsetky });
			foreach (var item in ZariadenieDataStore.GetItems(true))
			{
				Zariadenia.Add(item);
			}
		}


		public void ExecuteLoadItemsCommand()
		{
			IsBusy = true;

			try
			{
				Items.Clear();
				IQueryable<Models.Polozka> items = DataStore.GetItems(true).OrderBy(o => o.Expiracia).AsQueryable();

				if (SelectedZariadenie > 0)
					items = items.Where(o => o.Zariadenie == SelectedZariadenie);

				foreach (var item in items)
				{
					Items.Add(item);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}

		async void OnItemSelected(Models.Polozka item)
		{
			if (item == null)
				return;

			var p = DataStore.GetItems(false).FirstOrDefault(o => o.Id == item.Id);
			if (p != null)
			{
				bool answer = await contentPage.DisplayAlert(Resources.AppResources.vymazaniezaznamu, Resources.AppResources.naozajchcetevymazatzaznam, Resources.AppResources.ano, Resources.AppResources.nie);
				if (answer)
				{
					DataStore.DeleteItem(p.Id);
					ExecuteLoadItemsCommand();
					DMToast dt = new DMToast();
					dt.ToastMessage(Mraznicka.Resources.AppResources.polozka_bola_vymazana);

                    //Shell.Current.GoToAsync("..");
                }
            }

			// This will pop the current page off the navigation stack


			// This will push the ItemDetailPage onto the navigation stack
			//await Shell.Current.GoToAsync($"MiestnostDetailPage?{nameof(ViewModels.Miestnost.DetailViewModel.ItemId)}={item.Id}");
		}

	}
}
