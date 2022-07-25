using Mraznicka.Services;
using Mraznicka.ViewModels;
using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views.Vlozenie
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AllInOnePageTag : ContentPage
	{
		bool _eventsAlreadySubscribed = false;

		private bool _nfcIsEnabled;
		public bool NfcIsEnabled
		{
			get => _nfcIsEnabled;
			set
			{
				_nfcIsEnabled = value;
				OnPropertyChanged(nameof(NfcIsEnabled));
				OnPropertyChanged(nameof(NfcIsDisabled));
			}
		}

		public bool DeviceIsListening
		{
			get => _deviceIsListening;
			set
			{
				_deviceIsListening = value;
				OnPropertyChanged(nameof(DeviceIsListening));
			}
		}
		private bool _deviceIsListening;

		public bool NfcIsDisabled => !NfcIsEnabled;

		public const string ALERT_TITLE = "NFC";
		public const string MIME_TYPE = "application/sk.qsoft.iqfridge";

		bool _isDeviceiOS = false;

		public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();
		public IDataStore<Models.Tovar> DataStoreTovar => DependencyService.Get<IDataStore<Models.Tovar>>();
		public Command SaveCommand { get; }
		public Models.Polozka Item { get; set; }

		public AllInOnePageTag()
		{
			InitializeComponent();

			Item = ((App)Application.Current).AllInOneItem;
			SaveCommand = new Command(OnSave);

			if (CrossNFC.IsSupported)
			{
				if (!CrossNFC.Current.IsAvailable)
				{
					//await ShowAlert(Mraznicka.Resources.AppResources.nfcisnotavailable);
					DisplayAlert("Chytra Mraznicka", Mraznicka.Resources.AppResources.nfcisnotavailable, "Zrusit");
				}


				NfcIsEnabled = CrossNFC.Current.IsEnabled;
				if (!NfcIsEnabled)
				{
					DisplayAlert("Chytra Mraznicka", Mraznicka.Resources.AppResources.nfcisdissabled, "Zrusit");
				}


				if (Device.RuntimePlatform == Device.iOS)
					_isDeviceiOS = true;

			}


			this.BindingContext = this;
		}

		private bool ValidateSave()
		{
			return !String.IsNullOrWhiteSpace(Item.TagID);
		}

		private async void OnSave()
		{
			DataStore.AddItem(Item);
			//This will pop the current page off the navigation stack
			//await Shell.Current.GoToAsync("MainPage");

			MainThread.BeginInvokeOnMainThread(() =>
			{
				DisplayAlert("Chytra Mraznicka", "Zápis prebehol úspešne", "Ok").ContinueWith(t =>
				{
					Shell.Current.Navigation.PopToRootAsync();
				});
			});

			//await Shell.Current.Navigation.PopToRootAsync();
		}

		protected override void OnAppearing()
		{
			this.SubscribeEvents();
			CrossNFC.Current.StartListening();
			CrossNFC.Current.StartPublishing(true);
			base.OnAppearing();
		}


		protected override void OnDisappearing()
		{
			this.UnsubscribeEvents();
			CrossNFC.Current.StopListening();
			base.OnDisappearing();
		}

		public void SubscribeEvents()
		{
			if (_eventsAlreadySubscribed)
				return;

			_eventsAlreadySubscribed = true;

			CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
			CrossNFC.Current.OnMessagePublished += Current_OnMessagePublished;
			CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;
			CrossNFC.Current.OnNfcStatusChanged += Current_OnNfcStatusChanged;
			CrossNFC.Current.OnTagListeningStatusChanged += Current_OnTagListeningStatusChanged;

			if (_isDeviceiOS)
				CrossNFC.Current.OniOSReadingSessionCancelled += Current_OniOSReadingSessionCancelled;
		}

		public void UnsubscribeEvents()
		{
			CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;
			CrossNFC.Current.OnMessagePublished -= Current_OnMessagePublished;
			CrossNFC.Current.OnTagDiscovered -= Current_OnTagDiscovered;
			CrossNFC.Current.OnNfcStatusChanged -= Current_OnNfcStatusChanged;
			CrossNFC.Current.OnTagListeningStatusChanged -= Current_OnTagListeningStatusChanged;

			if (_isDeviceiOS)
				CrossNFC.Current.OniOSReadingSessionCancelled -= Current_OniOSReadingSessionCancelled;
		}

		void Current_OnTagListeningStatusChanged(bool isListening)
		{
			DeviceIsListening = isListening;
		}

		async void Current_OnNfcStatusChanged(bool isEnabled)
		{
			NfcIsEnabled = isEnabled;
			await DisplayAlert("Chytra Mraznicka", $"NFC has been {(isEnabled ? "enabled" : "disabled")}", "Zrusit");
		}


		void Current_OniOSReadingSessionCancelled(object sender, EventArgs e)
		{

		}

		async void Current_OnMessagePublished(ITagInfo tagInfo)
		{
			//CrossNFC.Current.StopPublishing();


		}


		async void Current_OnMessageReceived(ITagInfo tagInfo)
		{
		}


		async void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
		{
			if (!CrossNFC.Current.IsWritingTagSupported)
			{
				await DisplayAlert("Chytra Mraznicka", Mraznicka.Resources.AppResources.writingtagisnotsupportedonthisdevice, "Zrusit");
				return;
			}

			var identifier = tagInfo.Identifier;
			var serialNumber = NFCUtils.ByteArrayToHexString(identifier, ":");
			//ctx.Item.TagID = serialNumber;

			Debug.WriteLine(serialNumber);

			try
			{
				try
				{
					var exist = DataStore.GetItems(false).FirstOrDefault(o => o.TagID == tagInfo.SerialNumber);
					if (exist == null)
					{

						var tovar = DataStoreTovar.GetItem(Item.Tovar);
						var writeToTag = $@"{tovar.Nazov}
{Item.Popis}
Vložené:{Item.DatumVytvorenia.ToString("dd.MM.yyyy")}
Expirácia:{Item.Expiracia.ToString("dd.MM.yyyy")}
{Item.Hmotnost} g";

						NFCNdefRecord record = new NFCNdefRecord
						{
							TypeFormat = NFCNdefTypeFormat.WellKnown,
							MimeType = MIME_TYPE,

							Payload = NFCUtils.EncodeToByteArray(writeToTag),
							LanguageCode = "en"
						};

						tagInfo.Records = new[] { record };

						//CrossNFC.Current.ClearMessage(tagInfo);
						CrossNFC.Current.PublishMessage(tagInfo, false);



						Item.TagID = tagInfo.SerialNumber;
						SaveCommand.Execute(null);

						//TODO:
						//Item = new Models.Polozka();

						//await DisplayAlert("Chytra Mraznicka", Mraznicka.Resources.AppResources.writingtagoperationsuccessful, "Zrusit");

						

					}
					else
					{
						//Item = new Models.Polozka();
						await DisplayAlert("Chytra Mraznicka", "Dany tag uz je v databaze", "Zrusit");
					}

				}
				catch (Exception ex)
				{
					await DisplayAlert("Chytra Mraznicka", ex.Message, "Zrusit");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				await DisplayAlert("Chytra Mraznicka", ex.Message, "Zrusit");
			}
		}
	}
}
