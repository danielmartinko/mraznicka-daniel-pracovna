using Mraznicka.Models;
using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ViewModels.Vlozenie
{
	public class EANCodePageViewModel
	{
		public Command SaveCommand { get; }
		public Command CancelCommand { get; }
		public Models.Polozka Item { get; set; } = new Models.Polozka();
		public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();
		public IDataStore<Models.Tovar> DataStoreTovar => DependencyService.Get<IDataStore<Models.Tovar>>();

		public EANCodePageViewModel()
        {
			SaveCommand = new Command(OnSave, ValidateSave);
			CancelCommand = new Command(OnCancel);

			var pp = ((App)Application.Current).PoslednePouzite;
			var tovar = DataStoreTovar.GetItem(pp.Tovar);

			Item.PropertyChanged += (o, e) => SaveCommand.ChangeCanExecute();
			Item.TagID = String.Empty;
			Item.Expiracia = DateTime.Now.AddDays(tovar.Expiracia);
			Item.Miestnost = pp.Miestnost;
			Item.Pozicia = pp.Pozicia;
			Item.Tovar = pp.Tovar;
			Item.Zariadenie = pp.Zariadenie;
			Item.Typ = 2;
			Item.DatumVytvorenia = DateTime.Now;

		}

		private bool ValidateSave() 
		{
			//return !String.IsNullOrWhiteSpace(Item.Id.ToString())
			//	&& !String.IsNullOrWhiteSpace(Item.Nazov);

			return !String.IsNullOrWhiteSpace(Item.TagID) && !String.IsNullOrWhiteSpace(Item.Popis);
		}

		private async void OnCancel()
		{
			// This will pop the current page off the navigation stack
			await Shell.Current.GoToAsync("..");
		}

		private void OnSave()
		{
			DataStore.AddItem(Item);
			// This will pop the current page off the navigation stack
			Shell.Current.GoToAsync("..");
		}
	}
}
