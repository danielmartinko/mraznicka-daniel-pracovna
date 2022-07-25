using Mraznicka.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mraznicka.Views.Vlozenie
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManualPage : ContentPage
	{
		public ManualPage()
		{
			InitializeComponent();
			this.BindingContext = new ViewModels.Vlozenie.ManualPageViewModel();
		}
	}
}