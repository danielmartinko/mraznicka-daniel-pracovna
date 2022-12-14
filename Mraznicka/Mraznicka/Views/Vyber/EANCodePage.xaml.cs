using Mraznicka.ViewModels;
using Mraznicka.ViewModels.Vyber;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace Mraznicka.Views.Vyber
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EANCodePage : ContentPage
	{

		private EANCodePageViewModel ctx { get; set; }
		public EANCodePage()
		{
			InitializeComponent();
			this.BindingContext = this.ctx =  new ViewModels.Vyber.EANCodePageViewModel(this);
		}

		public void Handle_OnScanResult(Result result)
		{
			Device.BeginInvokeOnMainThread(async () =>
			{

				//var duration = TimeSpan.FromSeconds(1);
				//Vibration.Vibrate(duration);

				ISimpleAudioPlayer _simpleAudioPlayer;
				_simpleAudioPlayer = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
				System.IO.Stream beepStream = GetType().Assembly.GetManifestResourceStream("Mraznicka.beep-02.mp3");

				try
				{
					bool isSuccess = _simpleAudioPlayer.Load(beepStream);
					Vibration.Vibrate();
					_simpleAudioPlayer.Play();
				}
				catch (FeatureNotSupportedException ex)
				{
					// Feature not supported on device
				}



				//await DisplayAlert("Scanned result", result.Text, "OK");
				this.ctx.ExecuteLoadItemsCommand(result.Text);
			});
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			_scanView.IsScanning = true;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			_scanView.IsScanning = false;
		}
	}
}