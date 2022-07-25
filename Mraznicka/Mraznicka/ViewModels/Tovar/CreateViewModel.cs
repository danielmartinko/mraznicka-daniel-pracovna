using Mraznicka.Models;
using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ViewModels.Tovar
{
	public class CreateViewModel : BaseViewModel
	{
		public IDataStore<Models.Tovar> DataStore => DependencyService.Get<IDataStore<Models.Tovar>>();
		public List<Models.Tovar> Items { get; set; }
		public Models.Tovar Item { get; set; } = new Models.Tovar();
		public Command SaveCommand { get; }
		public Command CancelCommand { get; }
		public ContentPage contentPage { get; set; }

		public CreateViewModel(ContentPage page)
		{
			contentPage = page;
			SaveCommand = new Command(OnSave, ValidateSave);
			CancelCommand = new Command(OnCancel);

			Items = DataStore.GetItems(true).ToList();
			Item.PropertyChanged +=	(o,e ) => SaveCommand.ChangeCanExecute();
		}

		private bool ValidateSave()
		{
			//return !String.IsNullOrWhiteSpace(Item.Id.ToString())
			//	&& !String.IsNullOrWhiteSpace(Item.Nazov);

			return !String.IsNullOrWhiteSpace(Item.Nazov);
		}

	

		private void OnCancel()
		{
			// This will pop the current page off the navigation stack
			Shell.Current.GoToAsync("..");
		}

		private void OnSave()
		{
			var exist = Items.FirstOrDefault(o => o.Nazov == Item.Nazov) != null;
			if (exist)
			{
				contentPage.DisplayAlert(Resources.AppResources.zaznamexistuje, Resources.AppResources.zaznamexistuje, Resources.AppResources.zrusit);
			}
			else
			{
				DataStore.AddItem(Item);
				// This will pop the current page off the navigation stack
				Shell.Current.GoToAsync("..");
			}
		}
	}
}
