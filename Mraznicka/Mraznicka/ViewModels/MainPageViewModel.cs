using Mraznicka.Helpers;
using Mraznicka.Models;
using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ViewModels
{
	public class MainPageViewModel : BaseViewModel
	{
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

		public MainPageViewModel()
		{
			PushCommand = new Command(OnPushClicked, LicenseValidate);
			SaveCommand = new Command(OnSaveClicked, SaveValidate);
			PullCommand = new Command(OnPullClicked, LicenseValidate);
			PreviewCommand = new Command(OnPreviewClicked, LicenseValidate);
			Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			HasLicense = !LicenseValidate(null);
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
				"79123587"
			};

			return setting == null ? false : licences.Contains(setting.Val);

		}

		private bool SaveValidate(object arg)
		{
			return LicKey != "";
		}

		private async void OnSaveClicked(object obj)
		{
			DataStore.UpdateItem(new Setting() { Id = 1, Key = "LicenseKey", Val = LicKey });

			PushCommand.ChangeCanExecute();
			PullCommand.ChangeCanExecute();
			PreviewCommand.ChangeCanExecute();

			HasLicense = true;
		}

		private async void OnPushClicked(object obj)
		{
			await Shell.Current.GoToAsync("VlozenieAllInOnePage");
			//await Shell.Current.GoToAsync("PushPage");
			//await Shell.Current.GoToAsync("ExpressPush");
		}


		private async void OnPullClicked(object obj)
		{
			await Shell.Current.GoToAsync("PullPage");
			//await Shell.Current.GoToAsync("ExpressPull");
		}

		private async void OnPreviewClicked(object obj)
		{
			// Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
			//await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
			await Shell.Current.GoToAsync("PreviewPage");
		}


	}
}
