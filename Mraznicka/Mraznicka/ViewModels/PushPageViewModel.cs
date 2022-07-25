using Mraznicka.Models;
using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;


namespace Mraznicka.ViewModels
{
	public class PushPageViewModel
	{
		public Command EanCommand { get; }
		public Command ManualCommand { get; }
		public Command TagCommand { get; }

		public IDataStore<Models.PoslednePouzite> PoslednePouziteDataStore => DependencyService.Get<IDataStore<Models.PoslednePouzite>>();

		public ObservableCollection<Mraznicka.Models.Miestnost> Miestnosti { get; set; } = new ObservableCollection<Mraznicka.Models.Miestnost>();
		public ObservableCollection<Mraznicka.Models.Zariadenie> Zariadenia { get; set; } = new ObservableCollection<Mraznicka.Models.Zariadenie>();
		public ObservableCollection<Mraznicka.Models.Pozicia> Pozicie { get; set; } = new ObservableCollection<Mraznicka.Models.Pozicia>();
		public ObservableCollection<Mraznicka.Models.Tovar> Tovary { get; set; } = new ObservableCollection<Mraznicka.Models.Tovar>();

		public SelectedData SelectedData { get; set; } = new SelectedData();

		public PushPageViewModel()
		{
			EanCommand = new Command(OnEanClicked);
			ManualCommand = new Command(OnManualClicked);
			TagCommand = new Command(OnTagClicked);
			
		}


		private void SelectedData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var pp = ((App)Application.Current).PoslednePouzite;
			pp.Tovar = SelectedData.Tovar.Id;
			pp.Zariadenie = SelectedData.Zariadenie.Id;
			pp.Pozicia = SelectedData.Pozicia.Id;

			PoslednePouziteDataStore.UpdateItem(pp);
		}


		private async void OnEanClicked(object obj)
		{
			await Shell.Current.GoToAsync("VlozenieEanCodePage");
		}

		private async void OnManualClicked(object obj)
		{
			await Shell.Current.GoToAsync("VlozenieManualPage");
		}

		private async void OnTagClicked(object obj)
		{
			await Shell.Current.GoToAsync("VlozenieTagPage");
		}

		public void OnAppearing()
		{
			SelectedData.PropertyChanged += SelectedData_PropertyChanged;
		}
	}
}
