using Mraznicka.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views.Vyber
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompareManualPage : ContentPage
	{
		private ViewModels.Vyber.CompareManualPageViewModel ctx { get; set; }
		public CompareManualPage()
		{
			InitializeComponent();
			this.BindingContext = this.ctx = new ViewModels.Vyber.CompareManualPageViewModel(this);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			//this.ctx.ExecuteLoadItemsCommand();
		}
	}
}