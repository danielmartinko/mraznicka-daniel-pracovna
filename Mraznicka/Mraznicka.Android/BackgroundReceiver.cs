using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Mraznicka.Droid
{
    /*
    [BroadcastReceiver]
    public class BackgroundReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //PowerManager pm = (PowerManager)context.GetSystemService(Context.PowerService);
            //PowerManager.WakeLock wakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "BackgroundReceiver");
            //wakeLock.Acquire();

            MessagingCenter.Send<object, string>(this, "UpdateLabel", "Hello from Android");

            //wakeLock.Release();
        }
    }*/
}