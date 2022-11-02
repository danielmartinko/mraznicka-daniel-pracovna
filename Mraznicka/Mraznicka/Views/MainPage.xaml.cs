using Mraznicka.Interfaces;
using Mraznicka.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;
using Plugin.NFC;
using Plugin.Toast;
using Mraznicka.Services;
using Mraznicka.Models;
using System.Diagnostics;
using Xamarin.CommunityToolkit.Extensions;

namespace Mraznicka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
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

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new MainPageViewModel(this);

            CheckPermisson();
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

        protected override bool OnBackButtonPressed()
        {

            //if (Device.OS == TargetPlatform.Android)
            //{
            //	Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            //}

            if (Device.RuntimePlatform == Device.Android)
                DependencyService.Get<IAndroidMethods>().CloseApp();


            //return true;
            return base.OnBackButtonPressed();
        }

        public async void CheckPermisson()
        {
            var status = await CheckAndRequestPermissionAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

        }

        public static async Task<PermissionStatus> CheckAndRequestPermissionAsync<TPermission>() where TPermission : BasePermission, new()
        {
            TPermission permission = new TPermission();
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }
        protected override void OnAppearing()
        {
            this.SubscribeEvents();
            Task.Run(() => MainThread.BeginInvokeOnMainThread(() => 
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

        async void Current_OnMessagePublished(ITagInfo tagInfo)
        {
            //CrossNFC.Current.StopPublishing();
            await DisplayAlert(Mraznicka.Resources.AppResources.chytra_mraznicka, $"NFC message published" , Mraznicka.Resources.AppResources.zrusit);

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

            // string strPar = $"VyberTagPage?TAG_ID=" + tagInfo.SerialNumber + ",Meno=Daniel";
            // string strPar = $"VyberTagPage?Meno=Daniel";
            string strPar = $"VyberTagPage?TAG_ID=" + tagInfo.SerialNumber;
            Device.BeginInvokeOnMainThread(() =>
            {
                UnsubscribeEvents();
                CrossNFC.Current.StopListening();
            });
            await Shell.Current.GoToAsync(strPar);
        }

    }
}