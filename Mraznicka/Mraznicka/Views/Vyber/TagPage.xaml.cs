using Mraznicka.ViewModels;
using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
/*
 
 https://github.com/franckbour/Plugin.NFC
 
 */
namespace Mraznicka.Views.Vyber
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(TAG_ID), "TAG_ID")]
    public partial class TagPage : ContentPage, INotifyPropertyChanged
	{
		string tag_id;
        public string TAG_ID
        {
            set
            {
                tag_id = value;
            }
			get { return tag_id; }
        }
        string Name;
        public string NAME
        {
            set
            {
                Name = value;
            }
            get { return Name; }
        }
        public ViewModels.Vyber.TagPageViewModel ctx { get; set; }

		public TagPage()
		{
			InitializeComponent();
			this.BindingContext = this.ctx = new ViewModels.Vyber.TagPageViewModel(this);
			
		}

		protected override void OnAppearing()
		{
            this.ctx.LoadItem(TAG_ID);

            Task.Run(() => MainThread.BeginInvokeOnMainThread(() =>
            {
				this.ctx.SubscribeEvents();
				CrossNFC.Current.StartListening();
				CrossNFC.Current.StartPublishing(true);
			}));
			
			base.OnAppearing();
		}

		protected override bool OnBackButtonPressed()
		{
			this.ctx.UnsubscribeEvents();
			CrossNFC.Current.StopListening();
			return base.OnBackButtonPressed();
		}
		protected override void OnDisappearing()
		{
			// ((ViewModels.Vyber.TagPageViewModel)this.ctx).UnsubscribeEvents();
			// CrossNFC.Current.StopListening();
			base.OnDisappearing();
		}

	}
}