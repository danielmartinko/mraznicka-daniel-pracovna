using Mraznicka.Views;
using Mraznicka.Helpers;
using Mraznicka.Models;
using Mraznicka.Services;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Mraznicka.ViewModels
{
	public class FactoryViewModel : BaseViewModel
	{
		public Command RestoreCommand { get; }
		public ContentPage contentPage { get; set; }
		public IDataStore<Models.Setting> DataStore => DependencyService.Get<IDataStore<Models.Setting>>();
		public FactoryViewModel(ContentPage page)
		{
			contentPage = page;
			RestoreCommand = new Command(OnRestoreClicked);
		}

		private void OnRestore()
		{
			var databaseFile = "MyData.db";
			var databasePath = Path.Combine(FileSystem.AppDataDirectory, databaseFile);
			var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
			var embeddedDatabaseStream = assembly.GetManifestResourceStream("Mraznicka.MyData.db");

			FileStream fileStreamToWrite = File.Create(databasePath);
			embeddedDatabaseStream.Seek(0, SeekOrigin.Begin);
			embeddedDatabaseStream.CopyTo(fileStreamToWrite);
			fileStreamToWrite.Close();
			Models.Polozka polozka = new Models.Polozka();
			polozka.TagIDPrecitany = "";
			DataStore.UpdateItem(new Setting() { Id = 1, Key = "LicenseKey", Val = "65041460" });
			((App)Application.Current).PrveSpustenie();
		}


		private async void OnRestoreClicked(object obj)
		{
			if(((App)Application.Current).Licencia)
            {
				bool answer = await contentPage.DisplayAlert(Resources.AppResources.factory_setting_otazka_title, Resources.AppResources.factory_setting_otazka_text, Resources.AppResources.ano, Resources.AppResources.nie);
				if (answer)
				{
					OnRestore();
				}
			}
			// Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
			await Shell.Current.GoToAsync("MainPage");
		}
	}
}
