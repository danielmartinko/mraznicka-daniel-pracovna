using Mraznicka.ViewModels.Vlozenie;
using Plugin.NFC;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/*
 
 https://github.com/franckbour/Plugin.NFC
 
 */

namespace Mraznicka.Views.Vlozenie
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TagPage : ContentPage
	{

		private TagPageViewModel ctx { get; set; }

		public TagPage()
		{
			InitializeComponent();
			this.BindingContext = this.ctx = new ViewModels.Vlozenie.TagPageViewModel(this);
		}



		protected override void OnDisappearing()
		{
			((TagPageViewModel)this.ctx).UnsubscribeEvents();
			CrossNFC.Current.StopListening();
			base.OnDisappearing();
		}
	}
}