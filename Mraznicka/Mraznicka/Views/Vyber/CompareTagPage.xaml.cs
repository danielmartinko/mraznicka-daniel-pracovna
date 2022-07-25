using Mraznicka.ViewModels;
using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
/*
 
 https://github.com/franckbour/Plugin.NFC
 
 */
namespace Mraznicka.Views.Vyber
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompareTagPage : ContentPage, INotifyPropertyChanged
	{
		public ViewModels.Vyber.CompareTagPageViewModel ctx { get; set; }

		public CompareTagPage()
		{
			InitializeComponent();
			this.BindingContext = this.ctx = new ViewModels.Vyber.CompareTagPageViewModel(this);
		}

		protected async override void OnAppearing()
		{
			this.ctx.SubscribeEvents();
			CrossNFC.Current.StartListening();
			CrossNFC.Current.StartPublishing(true);
			base.OnAppearing();
		}

		//protected override bool OnBackButtonPressed()
		//{
		//	this.ctx.UnsubscribeEvents();
		//	CrossNFC.Current.StopListening();
		//	return base.OnBackButtonPressed();
		//}
		protected override void OnDisappearing()
		{
			this.ctx.UnsubscribeEvents();
			CrossNFC.Current.StopListening();
			base.OnDisappearing();
		}

	}
}