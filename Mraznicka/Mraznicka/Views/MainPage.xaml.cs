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


namespace Mraznicka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new MainPageViewModel();

            CheckPermisson();
        }

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

    }
}