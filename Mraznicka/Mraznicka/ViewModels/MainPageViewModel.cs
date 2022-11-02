using Mraznicka.Helpers;
using Mraznicka.Models;
using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using System.Globalization;
using Plugin.Toast;
using Xamarin.Essentials;
using Xamarin.CommunityToolkit.Extensions;
using Plugin.NFC;

namespace Mraznicka.ViewModels
{
	public class MainPageViewModel : BaseViewModel
	{
		ContentPage content_page;

		public Command PushCommand { get; }
		public Command PullCommand { get; }
		public Command PreviewCommand { get; }
		public Command SaveCommand { get; }

		public string Version { get; set; }

		private bool hasLicense;
		public bool HasLicense {
			get {
				return hasLicense && !LicenseValidate(null);
			}
			//set { SetAndNotify(ref hasLicense, value, () => HasLicense); }
			set { SetProperty(ref hasLicense, value); }
		}

		private string licKey;
		public string LicKey
		{
			get { return licKey; }
			set
			{
				SetProperty(ref licKey, value);				
			}
		}

		public IDataStore<Models.Setting> DataStore => DependencyService.Get<IDataStore<Models.Setting>>();

		public MainPageViewModel(ContentPage page)
		{
			content_page = page;
			PushCommand = new Command(OnPushClicked, LicenseValidate);
			SaveCommand = new Command(OnSaveClicked, SaveValidate);
			PullCommand = new Command(OnPullClicked, LicenseValidate);
			PreviewCommand = new Command(OnPreviewClicked, LicenseValidate);
			Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			HasLicense = !LicenseValidate(null);
			((App)Application.Current).Licencia = LicenseValidate(null);
		}

		private bool LicenseValidate(object arg)
		{

			var setting = DataStore.GetItems(false).FirstOrDefault(x => x.Key == "LicenseKey");

			var licences = new string[] {
				"96587123",
				"15678965",
				"36541258",
				"75698468",
				"26798521",
				"69489357",
				"95487564",
				"65423687",
				"29768458",
				"65041460"
			};

			return setting == null ? false : licences.Contains(setting.Val);

		}

		private bool SaveValidate(object arg)
		{
			return LicKey != "";
		}

		private void OnSaveClicked(object obj)
		{
			HasLicense = true;
			DataStore.UpdateItem(new Setting() { Id = 1, Key = "LicenseKey", Val = LicKey });

			PushCommand.ChangeCanExecute();
			PullCommand.ChangeCanExecute();
			PreviewCommand.ChangeCanExecute();
			if (LicenseValidate(null))
			{
				Button button = content_page.FindByName<Button>("button_ulozit");
				button.IsVisible = false;
				Label label = content_page.FindByName<Label>("label_licencne_cislo");
				label.IsVisible = false;
				Entry entry = content_page.FindByName<Entry>("entry_licencne_cislo");
				entry.IsVisible = false;
                DMToast dt = new DMToast();
                dt.ToastMessage(Mraznicka.Resources.AppResources.registracia_prebehla_uspesne);
            }
            else
            {
				// CrossToastPopUp.Current.ShowToastError(Mraznicka.Resources.AppResources.neplatny_licencny_kluc, Plugin.Toast.Abstractions.ToastLength.Long);
				DMToast dt = new DMToast();
				dt.ToastError(Mraznicka.Resources.AppResources.neplatny_licencny_kluc);

            }
        }

		private async void OnPushClicked(object obj)
		{
            Device.BeginInvokeOnMainThread(() =>
            {
                CrossNFC.Current.StopListening();
            });
            await Shell.Current.GoToAsync("VlozenieAllInOnePage");
			//await Shell.Current.GoToAsync("PushPage");
			//await Shell.Current.GoToAsync("ExpressPush");
		}


		private async void OnPullClicked(object obj)
		{
            Device.BeginInvokeOnMainThread(() =>
            {
                CrossNFC.Current.StopListening();
            });

            await Shell.Current.GoToAsync("PullPage");
			//await Shell.Current.GoToAsync("ExpressPull");
		}

		private async void OnPreviewClicked(object obj)
		{
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            Device.BeginInvokeOnMainThread(() =>
            {
                CrossNFC.Current.StopListening();
            });
            await Shell.Current.GoToAsync("PreviewPage");
		}


	}
}
