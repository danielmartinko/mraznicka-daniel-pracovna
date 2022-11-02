using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DMContentPage : ContentPage
    {
        bool _eventsAlreadySubscribed = false;
        bool _isDeviceiOS = false;


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

        public DMContentPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            this.SubscribeEvents();
            Task.Run(() => Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    CrossNFC.Current.StartListening();
                    CrossNFC.Current.StartPublishing(true);
                }
                catch (Exception ex)
                {

                }
                base.OnAppearing();
            }));
        }


        protected override void OnDisappearing()
        {
            //this.UnsubscribeEvents();
            //CrossNFC.Current.StopListening();
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

            _eventsAlreadySubscribed = false;
        }

        async void Current_OnMessagePublished(ITagInfo tagInfo)
        {
            //CrossNFC.Current.StopPublishing();
            await DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, $"NFC message published", Mraznicka.Resources.AppResources.zrusit);

        }


        async void Current_OnMessageReceived(ITagInfo tagInfo)
        {
            await DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, $"NFC message received", Mraznicka.Resources.AppResources.zrusit);
        }


        async void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
        {
            if (!CrossNFC.Current.IsWritingTagSupported)
            {
                await DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, Mraznicka.Resources.AppResources.writingtagisnotsupportedonthisdevice, Mraznicka.Resources.AppResources.zrusit);
                return;
            }

            var identifier = tagInfo.Identifier;
            var serialNumber = NFCUtils.ByteArrayToHexString(identifier, ":");

            Debug.WriteLine("TAG ID = " + serialNumber);

            string strPar = $"VyberTagPage?TAG_ID=" + tagInfo.SerialNumber;
            Device.BeginInvokeOnMainThread(() =>
            {
                UnsubscribeEvents();
                CrossNFC.Current.StopListening();
            });
            await Shell.Current.GoToAsync(strPar);
        }

        void Current_OnTagListeningStatusChanged(bool isListening)
        {
            DeviceIsListening = isListening;
        }

        async void Current_OnNfcStatusChanged(bool isEnabled)
        {
            NfcIsEnabled = isEnabled;
            await DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, $"NFC has been {(isEnabled ? "enabled" : "disabled")}", Mraznicka.Resources.AppResources.zrusit);
        }


        void Current_OniOSReadingSessionCancelled(object sender, EventArgs e)
        {

        }

    }
}