using Mraznicka.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mraznicka
{
    public class DMToast
    {
        public DMToast()
        {

        }
        public void ToastMessage(string str)
        {
            ToastOptions to = new ToastOptions
            {
                BackgroundColor = Color.GreenYellow,
                Duration = TimeSpan.FromSeconds(4),
                CornerRadius = 50,
                
                MessageOptions = new MessageOptions
                {
                    Message = str,
                    Foreground = Color.Black,
                    
                }
            };
            MainThread.BeginInvokeOnMainThread(async () =>
                await Application.Current.MainPage.DisplayToastAsync( to ) );
        }
        public void ToastError(string str)
        {
            ToastOptions to = new ToastOptions
            {
                BackgroundColor = Color.IndianRed,
                Duration = TimeSpan.FromSeconds(4),
                CornerRadius = 50,
                

                MessageOptions = new MessageOptions
                {
                    Message = str,
                    Foreground = Color.Black,

                }
            };
            
            MainThread.BeginInvokeOnMainThread(async () =>
                await Application.Current.MainPage.DisplayToastAsync(to));
        }
        public void ToastSuccess(string str)
        {
            ToastOptions to = new ToastOptions
            {
                BackgroundColor = Color.YellowGreen,
                Duration = TimeSpan.FromSeconds(4),
                CornerRadius = 50,

                MessageOptions = new MessageOptions
                {
                    Message = str,
                    Foreground = Color.Black,

                }
            };
            MainThread.BeginInvokeOnMainThread(async () =>
                await Application.Current.MainPage.DisplayToastAsync(to));
        }
        public void ToastWarning(string str)
        {
            ToastOptions to = new ToastOptions
            {
                BackgroundColor = Color.AliceBlue,
                Duration = TimeSpan.FromSeconds(4),
                CornerRadius = 50,

                MessageOptions = new MessageOptions
                {
                    Message = str,
                    Foreground = Color.Black,

                }
            };
            MainThread.BeginInvokeOnMainThread(async () =>
                await Application.Current.MainPage.DisplayToastAsync(to));
        }
    }
}
