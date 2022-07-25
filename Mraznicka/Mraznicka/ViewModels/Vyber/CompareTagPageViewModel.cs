using Mraznicka;
using Mraznicka.Services;
using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.SimpleAudioPlayer;

namespace Mraznicka.ViewModels.Vyber
{
	[QueryProperty(nameof(ItemId), nameof(ItemId))]
	public class CompareTagPageViewModel : BaseViewModel
	{

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

		private Models.Polozka tagItem;
		public Models.Polozka TagItem
		{
			get { return tagItem; }
			set
			{
				SetProperty(ref tagItem, value);
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


		private int itemId;

		public int ItemId
		{
			get
			{
				return itemId;
			}
			set
			{
				itemId = value;
				LoadItemId(value);
			}
		}


		public CompareTagPageViewModel(ContentPage page)
        {
			contentPage = page;
			VyberCommand = new Command(OnVyber, ValidateVyber);


			if (CrossNFC.IsSupported)
			{
				if (!CrossNFC.Current.IsAvailable)
				{
					//await ShowAlert(Mraznicka.Resources.AppResources.nfcisnotavailable);
					contentPage.DisplayAlert("Chytra Mraznicka", Mraznicka.Resources.AppResources.nfcisnotavailable, "Zrusit");
				}


				NfcIsEnabled = CrossNFC.Current.IsEnabled;
				if (!NfcIsEnabled)
				{
					contentPage.DisplayAlert("Chytra Mraznicka", Mraznicka.Resources.AppResources.nfcisdissabled, "Zrusit");
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
			if (Item == null || TagItem == null)
				return false;


			return Item.TagID == TagItem.TagID;
		}

		private async void OnVyber()
		{
			var item = DataStore.GetItems(false).FirstOrDefault(o => o.TagID == TagItem.TagID);
			if (item != null)
			{
				bool answer = await contentPage.DisplayAlert(Resources.AppResources.vymazaniezaznamu, Resources.AppResources.naozajchcetevymazatzaznam, Resources.AppResources.ano, Resources.AppResources.nie);
				if (answer)
				{
					DataStore.DeleteItem(item.Id);


					await Shell.Current.Navigation.PopToRootAsync();
					//Shell.Current.GoToAsync("PreviewPage");
				}
			}
			
			// This will pop the current page off the navigation stack
			
		}

		public void LoadItemId(int itemId)
		{
			try
			{
				Item = DataStore.GetItem(itemId);
				VyberCommand.ChangeCanExecute();

			}
			catch (Exception)
			{
				Debug.WriteLine("Failed to Load Item");
			}
		}

		public void Compare(string tagId)
		{
			Button b = contentPage.FindByName<Button>("btnSubmit");
			Label lNajdena = contentPage.FindByName<Label>("Najdena");
			Label lPopis = contentPage.FindByName<Label>("Popis");
			ISimpleAudioPlayer _simpleAudioPlayer;
			_simpleAudioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
			var duration = TimeSpan.FromSeconds(1);
			System.IO.Stream beepStream = GetType().Assembly.GetManifestResourceStream("Mraznicka.beep-02.mp3");

			TagItem = DataStore.GetItems().FirstOrDefault(o => o.TagID == tagId);
			if(TagItem != null)
			{
				if (Item.TagID == TagItem.TagID)
				{
					Vibration.Vibrate(duration);
					b.BackgroundColor = Color.Orange;
					b.IsEnabled = true;
					lNajdena.TextColor = Color.Blue;
					lPopis.TextColor = Color.Blue;
					beepStream = GetType().Assembly.GetManifestResourceStream("Mraznicka.Pubg - ok.mp3");
				 }
				else
				{
					b.IsEnabled = false;
					lNajdena.TextColor = Color.Black;
					lPopis.TextColor = Color.Black;
					beepStream = GetType().Assembly.GetManifestResourceStream("Mraznicka.No No.mp3");
				}
			}
            else
            {

            }

			try
			{
				bool isSuccess = _simpleAudioPlayer.Load(beepStream);
				_simpleAudioPlayer.Play();
				Vibration.Vibrate(duration);
			}
			catch (FeatureNotSupportedException ex)
			{
				// Feature not supported on device
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
		}

		void Current_OnTagListeningStatusChanged(bool isListening)
		{
			DeviceIsListening = isListening;
		}

		async void Current_OnNfcStatusChanged(bool isEnabled)
		{
			NfcIsEnabled = isEnabled;
			await contentPage.DisplayAlert("Chytra Mraznicka", $"NFC has been {(isEnabled ? "enabled" : "disabled")}", "Zrusit");
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
				Compare(tagInfo.SerialNumber);
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
				Compare(tagInfo.SerialNumber);
			}
			catch (Exception ex)
			{
				//problem s tagom napr (tagInfo.Records[0] nieje vyplnene)
			}
		}
	}
}
