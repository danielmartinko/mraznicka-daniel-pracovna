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

namespace Mraznicka.ViewModels.Tovar
{
	public class ListViewModel : BaseViewModel
	{
		public IDataStore<Models.Tovar> DataStore => DependencyService.Get<IDataStore<Models.Tovar>>();

		private Models.Tovar _selectedItem;

		public ObservableCollection<Models.Tovar> Items { get; }
		public Command LoadItemsCommand { get; }
		public Command AddItemCommand { get; }
		public Command<Models.Tovar> ItemTapped { get; }

		public ListViewModel()
		{
			Title = "Číselník Tovarov";
			Items = new ObservableCollection<Models.Tovar>();
			LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());

			ItemTapped = new Command<Models.Tovar>(OnItemSelected);

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

		public Models.Tovar SelectedItem
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
			await Shell.Current.GoToAsync("TovarCreatePage");
		}

		async void OnItemSelected(Models.Tovar item)
		{
			if (item == null)
				return;

			// This will push the ItemDetailPage onto the navigation stack
			await Shell.Current.GoToAsync($"TovarDetailPage?{nameof(ViewModels.Tovar.DetailViewModel.ItemId)}={item.Id}");
		}
	}
}
