using Mraznicka.Models;
using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ViewModels.Miestnost
{
	[QueryProperty(nameof(ItemId), nameof(ItemId))]
	public class DetailViewModel : BaseViewModel
	{
		public IDataStore<Models.Miestnost> DataStore => DependencyService.Get<IDataStore<Models.Miestnost>>();
		public Models.Miestnost Item { get; set; } = new Models.Miestnost();

		public Command SaveCommand { get; }
		public Command DeleteCommand { get; }

		private int itemId;

		public int ItemId
		{
			get
			{
				return itemId;
			}
			set
			{
				itemId = value;
				LoadItemId(value);
			}
		}

		public DetailViewModel()
		{
			Title = "Miestnosť";

			SaveCommand = new Command(OnSave, ValidateSave);
			DeleteCommand = new Command(OnDelete);

			Item.PropertyChanged += (o, e) => SaveCommand.ChangeCanExecute();
		}

		public void LoadItemId(int itemId)
		{
			try
			{
				var item = DataStore.GetItem(itemId);
				Item.Id = item.Id;
				Item.Nazov = item.Nazov;
			}
			catch (Exception)
			{
				Debug.WriteLine("Failed to Load Item");
			}
		}

		private bool ValidateSave()
		{
			//return !String.IsNullOrWhiteSpace(Item.Id.ToString())
			//	&& !String.IsNullOrWhiteSpace(Item.Nazov);

			return !String.IsNullOrWhiteSpace(Item.Nazov);
		}



		private void OnDelete()
		{
			DataStore.DeleteItem(Item.Id);
			// This will pop the current page off the navigation stack
			Shell.Current.GoToAsync("..");
		}

		private async void OnSave()
		{
			DataStore.UpdateItem(Item);
			// This will pop the current page off the navigation stack
			Shell.Current.GoToAsync("..");
		}
	}
}
