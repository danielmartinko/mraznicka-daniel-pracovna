using Mraznicka.Models;
using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ViewModels.Zariadenie
{
	public class CreateViewModel : BaseViewModel
	{
		public IDataStore<Models.Zariadenie> DataStore => DependencyService.Get<IDataStore<Models.Zariadenie>>();
		public List<Models.Zariadenie> Items { get; set; }
		public Models.Zariadenie Item { get; set; } = new Models.Zariadenie();
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



		private async void OnCancel()
		{
			// This will pop the current page off the navigation stack
			await Shell.Current.GoToAsync("..");
		}

		private void OnSave()
		{
			var exist = Items.FirstOrDefault(o => o.Nazov == Item.Nazov) != null;
			if (exist)
			{
				contentPage.DisplayAlert(Resources.AppResources.zaznamneexistuje, Resources.AppResources.zaznamneexistuje, Resources.AppResources.zrusit);
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
