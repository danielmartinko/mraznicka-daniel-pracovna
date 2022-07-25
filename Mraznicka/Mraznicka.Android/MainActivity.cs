using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Nfc;
using Android.Nfc.Tech;
using Android.Content;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Plugin.NFC;

namespace Mraznicka.Droid
{
    [Activity(Label = "Mraznicka", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private NfcAdapter _nfcAdapter;
        Mraznicka.App app;
        bool bZapisDoTagu = false;
        string co_zapisat_do_tagu;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _nfcAdapter = NfcAdapter.GetDefaultAdapter(this);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);


            // Plugin NFC: Initialization
            CrossNFC.Init(this);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            app = new App();
            LoadApplication( app );

            //MessagingCenter.Subscribe<Page, string>(this, "ZapisTAG", (sender, args) => {
            //    Console.WriteLine("######## Prislo ZapisTAG #######");
            //    Console.WriteLine(args);
            //    co_zapisat_do_tagu = args;
            //    bZapisDoTagu = true;

            //});


            //pokus 1
            //StartService(new Intent(this, typeof(PeriodicService)));

            //pokus 2
            /*
            var alarmIntent = new Intent(this, typeof(BackgroundReceiver));
			var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
			var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 3 * 1000, pending);
            */

            //pokus 3

            //SetAlarmForBackgroundServices(this);

        }

        //public static void SetAlarmForBackgroundServices(Context context)
        //{
        //    var alarmIntent = new Intent(context.ApplicationContext, typeof(AlarmReceiver));
        //    var broadcast = PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, PendingIntentFlags.NoCreate);
        //    if (broadcast == null)
        //    {
        //        var pendingIntent = PendingIntent.GetBroadcast(context.ApplicationContext, 0, alarmIntent, 0);
        //        var alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
        //        alarmManager.SetRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime(), 15000, pendingIntent);
        //    }
        //}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnPause()
        {
            base.OnPause();

            //if (_nfcAdapter != null)
            //    _nfcAdapter.DisableForegroundDispatch(this);
        }
        protected override void OnResume()
        {
            base.OnResume();

            // Plugin NFC: Restart NFC listening on resume (needed for Android 10+) 
            CrossNFC.OnResume();

            //if (_nfcAdapter == null)
            //{
            //    var alert = new AlertDialog.Builder(this).Create();
            //    alert.SetMessage("NFC is not supported on this device.");
            //    alert.SetTitle("NFC Unavailable");
            //    alert.Show();
            //}
            //else
            //{
            //    var tagDetected = new IntentFilter(NfcAdapter.ActionTagDiscovered);
            //    var ndefDetected = new IntentFilter(NfcAdapter.ActionNdefDiscovered);
            //    var techDetected = new IntentFilter(NfcAdapter.ActionTechDiscovered);

            //    var filters = new[] { ndefDetected, tagDetected, techDetected };

            //    var intent = new Intent(this, this.GetType()).AddFlags(ActivityFlags.SingleTop);

            //    var pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);

            //    _nfcAdapter.EnableForegroundDispatch(this, pendingIntent, filters, null);
            //}
        }

		protected override void OnNewIntent(Intent intent)
		{

            // Plugin NFC: Tag Discovery Interception
            CrossNFC.OnNewIntent(intent);

            //if (intent.Action == NfcAdapter.ActionTagDiscovered)
            //{
            //	string str_in_tag = "";
            //	string str_tag_id = "";
            //	var myTag = (Tag)intent.GetParcelableExtra(NfcAdapter.ExtraTag);
            //	if (myTag == null)
            //		return;
            //	var tagIdBytes = myTag.GetId();
            //	for (int i = 0; i < tagIdBytes.Length; i++)
            //	{
            //		str_tag_id += tagIdBytes[i].ToString("X2");
            //		if (i < (tagIdBytes.Length - 1))
            //			str_tag_id += "-";
            //	}

            //	if (bZapisDoTagu)
            //	{
            //		bZapisDoTagu = false;
            //		WriteToTag(intent, co_zapisat_do_tagu);
            //		app.mainPage.ZapisanyTag(str_tag_id);
            //		return;
            //	}

            //	var rawMessages = intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
            //	if (rawMessages != null)
            //	{
            //		var msg = (NdefMessage)rawMessages[0];
            //		// Get NdefRecord which contains the actual data
            //		for (int i = 0; i < msg.GetRecords().Length; i++)
            //		{
            //			var record = msg.GetRecords()[i];
            //			if (record == null)
            //				return;
            //			// The data is defined by the Record Type Definition (RTD) specification available from http://members.nfc-forum.org/specs/spec_list/
            //			if (record.Tnf != NdefRecord.TnfWellKnown)
            //				return;
            //			var data = Encoding.UTF8.GetString(record.GetPayload());
            //			str_in_tag = data;
            //			Console.WriteLine("@@@@@@@@@@@@");
            //			Console.WriteLine(str_in_tag);
            //			Console.WriteLine("@@@@@@@@@@@@");

            //			app.mainPage.PrecitanyTAG(str_tag_id, str_in_tag);
            //		}
            //	}
            //	else
            //	{
            //	}

            //	// alertMessage.SetMessage("ID: " + str_tag_id + "\r\n" + str_in_tag); // Here's the id of card
            //	// alertMessage.Show();

            //	// WriteToTag(intent, "Žšľčťžýáíé");
            //}
        }

		public void WriteToTag(Intent intent, string content)
        {
            //if (!(intent.GetParcelableExtra(NfcAdapter.ExtraTag) is Tag tag)) return;
            //Ndef ndef = Ndef.Get(tag);
            //if (ndef == null || !ndef.IsWritable) return;
            //var payload = Encoding.UTF8.GetBytes(content);
            //var mimeBytes = Encoding.UTF8.GetBytes("text/plain");
            //var record = new NdefRecord(NdefRecord.TnfWellKnown, mimeBytes, new byte[0], payload);
            //var ndefMessage = new NdefMessage(new[] { record });
            //try
            //{
            //    ndef.Connect();
            //    ndef.WriteNdefMessage(ndefMessage);
            //    ndef.Close();
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine("############");
            //    Console.WriteLine(ex.Message);
            //    Console.WriteLine("############");
            //}
        }

    }
}