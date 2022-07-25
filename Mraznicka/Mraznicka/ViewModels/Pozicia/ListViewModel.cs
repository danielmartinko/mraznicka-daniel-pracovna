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

namespace Mraznicka.ViewModels.Pozicia
{
	public class ListViewModel : BaseViewModel
	{
		public IDataStore<Models.Pozicia> DataStore => DependencyService.Get<IDataStore<Models.Pozicia>>();

		private Models.Pozicia _selectedItem;

		public ObservableCollection<Models.Pozicia> Items { get; }
		public Command LoadItemsCommand { get; }
		public Command AddItemCommand { get; }
		public Command<Models.Pozicia> ItemTapped { get; }

		public ListViewModel()
		{
			Title = "Číselník Pozicií";
			Items = new ObservableCollection<Models.Pozicia>();
			LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());

			ItemTapped = new Command<Models.Pozicia>(OnItemSelected);

			AddItemCommand = new Command(OnAddItem);
		}

		private void ExecuteLoadItemsCommand()
		{
			IsBusy = true;

			try
			{
				Items.Clear();
				var items =  DataStore.GetItems(true);
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

		public Models.Pozicia SelectedItem
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
			Shell.Current.GoToAsync("PoziciaCreatePage");
		}

		private void OnItemSelected(Models.Pozicia item)
		{
			if (item == null)
				return;

			// This will push the ItemDetailPage onto the navigation stack
			Shell.Current.GoToAsync($"PoziciaDetailPage?{nameof(ViewModels.Pozicia.DetailViewModel.ItemId)}={item.Id}");
		}
	}
}
