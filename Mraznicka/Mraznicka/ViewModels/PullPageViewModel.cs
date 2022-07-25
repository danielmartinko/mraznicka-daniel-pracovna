using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.ViewModels
{
	public class PullPageViewModel
	{
		public Command EanCommand { get; }
		public Command ManualCommand { get; }
		public Command TagCommand { get; }

		public PullPageViewModel()
        {
			EanCommand = new Command(OnEanClicked);
			ManualCommand = new Command(OnManualClicked);
			TagCommand = new Command(OnTagClicked);
		}


		private async void OnEanClicked(object obj)
		{
			await Shell.Current.GoToAsync("VyberEanCodePage");
		}

		private async void OnManualClicked(object obj)
		{
			await Shell.Current.GoToAsync("VyberManualPage");
		}

		private async void OnTagClicked(object obj)
		{
			await Shell.Current.GoToAsync("VyberTagPage");
		}
	}
}
