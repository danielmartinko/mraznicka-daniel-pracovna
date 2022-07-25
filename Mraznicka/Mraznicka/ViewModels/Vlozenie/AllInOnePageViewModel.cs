using Mraznicka.Models;
using Mraznicka.Services;
using Plugin.NFC;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;

namespace Mraznicka.ViewModels.Vlozenie
{
	public class AllInOnePageViewModel : BaseViewModel
	{
		public Command SaveCommand { get; }
		public Command ManualCommand { get; }
		public Command TagCommand { get; }
		public Command EanCommand { get; }
		public IDataStore<Models.PoslednePouzite> PoslednePouziteDataStore => DependencyService.Get<IDataStore<Models.PoslednePouzite>>();
		public ObservableCollection<Mraznicka.Models.Miestnost> Miestnosti { get; set; } = new ObservableCollection<Mraznicka.Models.Miestnost>();
		public ObservableCollection<Mraznicka.Models.Zariadenie> Zariadenia { get; set; } = new ObservableCollection<Mraznicka.Models.Zariadenie>();
		public ObservableCollection<Mraznicka.Models.Pozicia> Pozicie { get; set; } = new ObservableCollection<Mraznicka.Models.Pozicia>();
		public ObservableCollection<Mraznicka.Models.Tovar> Tovary { get; set; } = new ObservableCollection<Mraznicka.Models.Tovar>();
		public SelectedData SelectedData { get; set; } = new SelectedData();
		public Command CancelCommand { get; }
		public ContentPage contentPage { get; set; }
		public Models.Polozka Item { get; set; } = new Models.Polozka();
		public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();
		public IDataStore<Models.Tovar> DataStoreTovar => DependencyService.Get<IDataStore<Models.Tovar>>();

		public AllInOnePageViewModel(ContentPage page)
		{
			contentPage = page;

			var pp = ((App)Application.Current).PoslednePouzite;
			
			SaveCommand = new Command(OnSave, ValidateSave);
			ManualCommand = new Command(OnManual, ValidateManual);
			TagCommand = new Command(OnTag, ValidateTag);
			EanCommand = new Command(OnEan, ValidateEan);

			CancelCommand = new Command(OnCancel);

			Item.PropertyChanged += (o, e) =>
			{
				if(e.PropertyName == "Tovar")
				{ 
					var tovar = DataStoreTovar.GetItem(Item.Tovar);
					
					if(tovar != null)
						Item.Expiracia = DateTime.Now.AddDays(tovar.Expiracia);
					
				}
				SaveCommand.ChangeCanExecute();
				ManualCommand.ChangeCanExecute();
				EanCommand.ChangeCanExecute();
				TagCommand.ChangeCanExecute();
			};

			Item.TagID = String.Empty;
			Item.Miestnost = pp.Miestnost;
			Item.DatumVytvorenia = DateTime.Now;
			Item.Tovar = pp.Tovar;
			Item.Zariadenie = pp.Zariadenie;
			Item.Pozicia = pp.Pozicia;
			
		}

		private bool ValidateSave()
		{
			return !String.IsNullOrWhiteSpace(Item.Popis);
		}

		private bool ValidateManual()
		{
			return !String.IsNullOrWhiteSpace(Item.Popis);
		}

		private bool ValidateTag()
		{
			return !String.IsNullOrWhiteSpace(Item.Popis);
		}

		private bool ValidateEan()
		{
			return !String.IsNullOrWhiteSpace(Item.Popis);
		}

		private void OnCancel()
		{
			// This will pop the current page off the navigation stack
			Shell.Current.GoToAsync("..");
		}

		private async void OnSave()
		{
			DataStore.AddItem(Item);
			// This will pop the current page off the navigation stack
			//Shell.Current.GoToAsync("..");


			MainThread.BeginInvokeOnMainThread(() =>
			{
				contentPage.DisplayAlert("Chytra Mraznicka", "Zápis prebehol úspešne", "Ok").ContinueWith(t =>
				{
					Shell.Current.Navigation.PopToRootAsync();
				});
			});
			//await Shell.Current.Navigation.PopToRootAsync();
		}

		private void OnManual()
		{
			Item.Typ = 3;
			SaveCommand.Execute(null);
		}

		private async void OnTag()
		{
			Item.Typ = 1;
			((App)Application.Current).AllInOneItem = Item;
			await Shell.Current.GoToAsync("VlozenieAllInOnePageTag");
		}

		private async void OnEan()
		{
			Item.Typ = 2;
			((App)Application.Current).AllInOneItem = Item;
			await Shell.Current.GoToAsync("VlozenieAllInOnePageEan");


		}

		public void OnAppearing()
		{
			SelectedData.PropertyChanged += SelectedData_PropertyChanged;
		}

		private void SelectedData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var pp = ((App)Application.Current).PoslednePouzite;

			pp.Tovar = Item.Tovar = SelectedData.Tovar.Id;
			pp.Zariadenie = Item.Zariadenie = SelectedData.Zariadenie.Id;
			pp.Pozicia = Item.Pozicia = SelectedData.Pozicia.Id;

			PoslednePouziteDataStore.UpdateItem(pp);
		}

	}
}
