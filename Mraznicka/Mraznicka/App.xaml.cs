using Xamarin.Forms;
using Mraznicka.Models;
using Mraznicka.Services;
using System.Reflection;
using System;
using System.IO;
using Xamarin.Essentials;
using Plugin.NFC;

namespace Mraznicka
{
	public struct Constant
	{
		//WidthRequest="{Binding Source={x:Static local:Constant.ScreenWidth}}"
		public static double ScreenWidth = Application.Current.MainPage.Width;
		public static double ScreenHeight = Application.Current.MainPage.Height;
	}



	public partial class App : Application
	{

		public IDataStore<Models.PoslednePouzite> PoslednePouziteDataStore => DependencyService.Get<IDataStore<Models.PoslednePouzite>>();
		public IDataStore<Models.Zariadenie> ZariadenieDataStore => DependencyService.Get<IDataStore<Models.Zariadenie>>();

		public PoslednePouzite PoslednePouzite { get; set; }

		public Polozka AllInOneItem { get; set; }

		public App()
		{
			InitializeComponent();

			DependencyService.Register<MiestnostDataStore>();
			DependencyService.Register<PoziciaDataStore>();
			DependencyService.Register<TovarDataStore>();
			DependencyService.Register<ZariadenieDataStore>();
			DependencyService.Register<PolozkaDataStore>();
			DependencyService.Register<PoslednePouziteDataStore>();
			DependencyService.Register<SettingDataStore>();

			var databaseFile = "MyData.db";
			var databasePath = Path.Combine(FileSystem.AppDataDirectory, databaseFile);
			var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
			var embeddedDatabaseStream = assembly.GetManifestResourceStream("Mraznicka.MyData.db");
			if (!File.Exists(databasePath))
			{ 
				FileStream fileStreamToWrite = File.Create(databasePath);
				embeddedDatabaseStream.Seek(0, SeekOrigin.Begin);
				embeddedDatabaseStream.CopyTo(fileStreamToWrite);
				fileStreamToWrite.Close();
			}



			MainPage = new AppShell();
			PoslednePouzite = PoslednePouziteDataStore.GetItem(1);


			MessagingCenter.Subscribe<object, string>(this, "UpdateLabel", (s, e) =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{
				
				});
			});

			

		}

		public void SetLastUsed()
		{



		}

		protected override void OnStart()
		{

			//rezervovanie si nfc
			//CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
			//CrossNFC.Current.OnMessagePublished += Current_OnMessagePublished;
			//CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;
			//CrossNFC.Current.OnNfcStatusChanged += Current_OnNfcStatusChanged;
			//CrossNFC.Current.OnTagListeningStatusChanged += Current_OnTagListeningStatusChanged;
		}

		private void Current_OnTagListeningStatusChanged(bool isListening)
		{
		}

		private void Current_OnNfcStatusChanged(bool isEnabled)
		{
		}

		private void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
		{
		}

		private void Current_OnMessagePublished(ITagInfo tagInfo)
		{
		}

		private void Current_OnMessageReceived(ITagInfo tagInfo)
		{
		}

	
		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
			try
			{
				//rezervovanie si nfc
				CrossNFC.Current.StartListening();
			}
			catch (Exception ex)
			{ 
			}
			
		}
	}
}
