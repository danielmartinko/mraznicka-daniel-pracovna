using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ViewModels.Vyber
{
	public class ManualPageViewModel : BaseViewModel
	{
		public Command<Models.Polozka> ItemTapped { get; }

		public ObservableCollection<Models.Polozka> Items { get; }

		public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();

		public ContentPage contentPage { get; set; }

		public ManualPageViewModel(ContentPage page)
		{
			contentPage = page;
			Items = new ObservableCollection<Models.Polozka>();
			ItemTapped = new Command<Models.Polozka>(OnItemSelected);
		}


		public void ExecuteLoadItemsCommand()
		{
			IsBusy = true;

			try
			{
				Items.Clear();
				var items = DataStore.GetItems(true);
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
					Shell.Current.GoToAsync("..");
				}
			}

			// This will pop the current page off the navigation stack


			// This will push the ItemDetailPage onto the navigation stack
			//await Shell.Current.GoToAsync($"MiestnostDetailPage?{nameof(ViewModels.Miestnost.DetailViewModel.ItemId)}={item.Id}");
		}

	}
}
