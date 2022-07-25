using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mraznicka.Droid;
using Mraznicka.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidMethods))]
namespace Mraznicka.Droid
{
    public class AndroidMethods : IAndroidMethods
    {
        public void CloseApp()
        {
            //var activity = (Activity)Forms.Context;
            //activity.FinishAffinity();

            //Android.OS.Process.KillProcess(Android.OS.Process.MyPid());


            Activity activity = (Activity)Forms.Context;
            activity.FinishAndRemoveTask();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            Java.Lang.JavaSystem.Exit(0);


            /*
                For Android: Activity activity = (Activity)Forms.Context; activity.FinishAffinity(); JavaSystem.Exit(0);
                For iOS and UWP: Process.GetCurrentProcess().Kill();
             */
        }
    }
}