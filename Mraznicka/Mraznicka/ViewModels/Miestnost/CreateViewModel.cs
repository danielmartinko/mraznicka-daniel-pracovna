using Mraznicka.Models;
using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ViewModels.Miestnost
{
	public class CreateViewModel : BaseViewModel
	{
		public IDataStore<Models.Miestnost> DataStore => DependencyService.Get<IDataStore<Models.Miestnost>>();
		public Models.Miestnost Item { get; set; } = new Models.Miestnost();

		public CreateViewModel()
		{
			SaveCommand = new Command(OnSave, ValidateSave);
			CancelCommand = new Command(OnCancel);

			Item.PropertyChanged +=	(o,e ) => SaveCommand.ChangeCanExecute();
		}

		private bool ValidateSave()
		{
			//return !String.IsNullOrWhiteSpace(Item.Id.ToString())
			//	&& !String.IsNullOrWhiteSpace(Item.Nazov);

			return !String.IsNullOrWhiteSpace(Item.Nazov);
		}

		public Command SaveCommand { get; }
		public Command CancelCommand { get; }

		private void OnCancel()
		{
			// This will pop the current page off the navigation stack
			Shell.Current.GoToAsync("..");
		}

		private void OnSave()
		{
			DataStore.AddItem(Item);
			// This will pop the current page off the navigation stack
			Shell.Current.GoToAsync("..");
		}
	}
}
