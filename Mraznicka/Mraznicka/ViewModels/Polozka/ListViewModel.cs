using Mraznicka.Models;
using Mraznicka.Services;
using Mraznicka.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mraznicka.ViewModels.Polozka
{
	public class ListViewModel : BaseViewModel
	{
		public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();

		private Models.Polozka _selectedItem;

		public ObservableCollection<Models.Polozka> Items { get; }
		public Command LoadItemsCommand { get; }
		public Command AddItemCommand { get; }
		public Command<Models.Polozka> ItemTapped { get; }

		public ListViewModel()
		{
			Title = "Číselník Poloziek";
			Items = new ObservableCollection<Models.Polozka>();
			LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());

			ItemTapped = new Command<Models.Polozka>(OnItemSelected);

			AddItemCommand = new Command(OnAddItem);
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

		public void OnAppearing()
		{
			IsBusy = true;
			SelectedItem = null;
		}

		public Models.Polozka SelectedItem
		{
			get => _selectedItem;
			set
			{
				SetProperty(ref _selectedItem, value);
				OnItemSelected(value);
			}
		}

		private void OnAddItem(object obj)
		{
			Shell.Current.GoToAsync("PolozkaCreatePage");
		}

		void OnItemSelected(Models.Polozka item)
		{
			if (item == null)
				return;

			// This will push the ItemDetailPage onto the navigation stack
			 Shell.Current.GoToAsync($"PolozkaDetailPage?{nameof(ViewModels.Polozka.DetailViewModel.ItemId)}={item.Id}");
		}
	}
}
