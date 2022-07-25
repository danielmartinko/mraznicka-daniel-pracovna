using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mraznicka.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{
		public AboutViewModel()
		{
			Title = "O mrazničke";
			OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://chytramraznicka.sk/"));
		}

		public ICommand OpenWebCommand { get; }
	}
}
