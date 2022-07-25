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

namespace Mraznicka.ViewModels.Miestnost
{
	public class ListViewModel : BaseViewModel
	{
		public IDataStore<Models.Miestnost> DataStore => DependencyService.Get<IDataStore<Models.Miestnost>>();

		private Models.Miestnost _selectedItem;

		public ObservableCollection<Models.Miestnost> Items { get; }
		public Command LoadItemsCommand { get; }
		public Command AddItemCommand { get; }
		public Command<Models.Miestnost> ItemTapped { get; }

		public ListViewModel()
		{
			Title = "Číselník miestností";
			Items = new ObservableCollection<Models.Miestnost>();
			LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());

			ItemTapped = new Command<Models.Miestnost>(OnItemSelected);

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

		public Models.Miestnost SelectedItem
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
			await Shell.Current.GoToAsync("MiestnostCreatePage");
		}

		async void OnItemSelected(Models.Miestnost item)
		{
			if (item == null)
				return;

			// This will push the ItemDetailPage onto the navigation stack
			await Shell.Current.GoToAsync($"MiestnostDetailPage?{nameof(ViewModels.Miestnost.DetailViewModel.ItemId)}={item.Id}");
		}
	}
}
