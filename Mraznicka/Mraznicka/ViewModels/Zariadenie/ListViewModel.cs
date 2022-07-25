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

namespace Mraznicka.ViewModels.Zariadenie
{
	public class ListViewModel : BaseViewModel
	{
		public IDataStore<Models.Zariadenie> DataStore => DependencyService.Get<IDataStore<Models.Zariadenie>>();

		private Models.Zariadenie _selectedItem;

		public ObservableCollection<Models.Zariadenie> Items { get; }
		public Command LoadItemsCommand { get; }
		public Command AddItemCommand { get; }
		public Command<Models.Zariadenie> ItemTapped { get; }

		public ListViewModel()
		{
			Title = "Číselník Zariadení";
			Items = new ObservableCollection<Models.Zariadenie>();
			LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());

			ItemTapped = new Command<Models.Zariadenie>(OnItemSelected);

			AddItemCommand = new Command(OnAddItem);
		}

		private void ExecuteLoadItemsCommand()
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

		public Models.Zariadenie SelectedItem
		{
			get => _selectedItem;
			set
			{
				SetProperty(ref _selectedItem, value);
				OnItemSelected(value);
			}
		}

		private async void OnAddItem(object obj)
		{
			await Shell.Current.GoToAsync("ZariadenieCreatePage");
		}

		async void OnItemSelected(Models.Zariadenie item)
		{
			if (item == null)
				return;

			// This will push the ItemDetailPage onto the navigation stack
			await Shell.Current.GoToAsync($"ZariadenieDetailPage?{nameof(ViewModels.Zariadenie.DetailViewModel.ItemId)}={item.Id}");
		}
	}
}
