using Mraznicka.Services;
using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using Mraznicka.Models;
using Plugin.SimpleAudioPlayer;
using Plugin.Toast;
using Xamarin.CommunityToolkit.Extensions;

namespace Mraznicka.ViewModels.Vyber
{

	//[QueryProperty(nameof(ItemId), nameof(ItemId))]
	public class TagPageViewModel : BaseViewModel
	{
		private Label hmotnost;
		private Label hmotnost_g;
		private Label vlozene;
		private Label najdena_polozka;

		public Command<Models.Polozka> ItemTapped { get; }
		public IDataStore<Models.Polozka> DataStore => DependencyService.Get<IDataStore<Models.Polozka>>();

		private Models.Polozka item;
		public Models.Polozka Item
		{
			get { return item; }
			set
			{
				SetProperty(ref item, value);
			}
		}


		public Command VyberCommand { get; }

		public ContentPage contentPage { get; set; }

		public const string ALERT_TITLE = "NFC";
		public const string MIME_TYPE = "application/sk.qsoft.iqfridge";

		NFCNdefTypeFormat _type;
		bool _makeReadOnly = false;
		bool _eventsAlreadySubscribed = false;
		bool _isDeviceiOS = false;

		/// <summary>
		/// Property that tracks whether the Android device is still listening,
		/// so it can indicate that to the user.
		/// </summary>
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

		public bool NfcIsDisabled => !NfcIsEnabled;



		public TagPageViewModel(ContentPage page)
        {
			contentPage = page;
			VyberCommand = new Command(OnVyber, ValidateVyber);
			ItemTapped = new Command<Models.Polozka>(OnItemSelected);
			hmotnost = contentPage.FindByName<Label>("hmotnost");
			hmotnost_g = contentPage.FindByName<Label>("hmotnost_g");
			vlozene = contentPage.FindByName<Label>("vlozene");
			najdena_polozka = contentPage.FindByName<Label>("najdena_polozka");
			vlozene.IsVisible = false;
			hmotnost.IsVisible = false;
			hmotnost_g.IsVisible = false;
			najdena_polozka.IsVisible = false;

			if (CrossNFC.IsSupported)
			{
				if (!CrossNFC.Current.IsAvailable)
				{
					//await ShowAlert(Mraznicka.Resources.AppResources.nfcisnotavailable);
					contentPage.DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, Mraznicka.Resources.AppResources.nfcisnotavailable, Mraznicka.Resources.AppResources.zrusit);
				}


				NfcIsEnabled = CrossNFC.Current.IsEnabled;
				if (!NfcIsEnabled)
				{
					contentPage.DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, Mraznicka.Resources.AppResources.nfcisdissabled, Mraznicka.Resources.AppResources.zrusit);
				}


				if (Device.RuntimePlatform == Device.iOS)
					_isDeviceiOS = true;


			}
		}

		private bool ValidateVyber()
		{
			//return !String.IsNullOrWhiteSpace(Item.Id.ToString())
			//	&& !String.IsNullOrWhiteSpace(Item.Nazov);

			//return !String.IsNullOrWhiteSpace(Item.Tovar);

			return Item != null;
		}

		private async void OnVyber()
		{

			bool answer = await contentPage.DisplayAlert(Resources.AppResources.vymazaniezaznamu, Resources.AppResources.naozajchcetevymazatzaznam, Resources.AppResources.ano, Resources.AppResources.nie);
			if (answer)
			{
                //CrossToastPopUp.Current.ShowToastSuccess(Resources.AppResources.polozka_tag_vymazana, Plugin.Toast.Abstractions.ToastLength.Long);
                DataStore.DeleteItem(Item.Id);
                DMToast dt = new DMToast();
                dt.ToastSuccess(Mraznicka.Resources.AppResources.polozka_tag_vymazana);
                await Shell.Current.GoToAsync("..");
			}
		}


		public async void LoadItem(string tagId)
		{
			Item = DataStore.GetItems().FirstOrDefault(o => o.TagID == tagId);
			if (Item != null)
			{
				//hmotnost.IsVisible = true;
				//hmotnost_g.IsVisible = true;
				vlozene.IsVisible = true;
				najdena_polozka.IsVisible = true;
				ISimpleAudioPlayer _simpleAudioPlayer;
				_simpleAudioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
				System.IO.Stream beepStream = GetType().Assembly.GetManifestResourceStream("Mraznicka.beep-02.mp3");

				try
				{
					bool isSuccess = _simpleAudioPlayer.Load(beepStream);
					_simpleAudioPlayer.Play();

					var duration = TimeSpan.FromSeconds(1);
					Vibration.Vibrate(duration);
				}
				catch (FeatureNotSupportedException ex)
				{
					// Feature not supported on device
				}
			}
			else
            {
				hmotnost.IsVisible = false;
				hmotnost_g.IsVisible = false;
				vlozene.IsVisible = false;
				najdena_polozka.IsVisible = false;
                //CrossToastPopUp.Current.ShowToastError(Resources.AppResources.tagsanepouziva, Plugin.Toast.Abstractions.ToastLength.Long);
                DMToast dt = new DMToast();
                dt.ToastError(Mraznicka.Resources.AppResources.tagsanepouziva);

            }

            VyberCommand.ChangeCanExecute();
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

            _eventsAlreadySubscribed = false;
        }

		void Current_OnTagListeningStatusChanged(bool isListening)
		{
			DeviceIsListening = isListening;
		}

		async void Current_OnNfcStatusChanged(bool isEnabled)
		{
			NfcIsEnabled = isEnabled;
			await contentPage.DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, $"NFC has been {(isEnabled ? "enabled" : "disabled")}", Mraznicka.Resources.AppResources.zrusit);
		}


		void Current_OniOSReadingSessionCancelled(object sender, EventArgs e)
		{

		}

		async void Current_OnMessageReceived(ITagInfo tagInfo)
		{
			var identifier = tagInfo.Identifier;
			var serialNumber = NFCUtils.ByteArrayToHexString(identifier, ":");

			try
			{
				LoadItem(tagInfo.SerialNumber);
			}
			catch (Exception ex)
			{
				//problem s tagom napr (tagInfo.Records[0] nieje vyplnene)
			}

		}

		async void Current_OnMessagePublished(ITagInfo tagInfo)
		{
		}

		async void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
		{
			var identifier = tagInfo.Identifier;
			var serialNumber = NFCUtils.ByteArrayToHexString(identifier, ":");

			try
			{
				LoadItem(tagInfo.SerialNumber);
			}
			catch (Exception ex)
			{
				//problem s tagom napr (tagInfo.Records[0] nieje vyplnene)
			}
		}

		async void OnItemSelected(Models.Polozka item)
		{
			if (item == null)
				return;

			// This will push the ItemDetailPage onto the navigation stack
			await Shell.Current.GoToAsync($"PolozkaDetailPage?{nameof(ViewModels.Polozka.DetailViewModel.ItemId)}={item.Id}");
		}
	}
}
