using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace Sokea_Project
{
    [Activity(Label = "Splash_Screen", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        static readonly List<string> randonlist = new List<string>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.gonext);
            button.Click += (object sender, EventArgs e) =>
            {
                Intent intent = new Intent(this, typeof(MainScreenActivity));
                intent.PutStringArrayListExtra("Holamundo", randonlist);
                StartActivity(intent);
            };

            Button button2 = FindViewById<Button>(Resource.Id.gonext2);
            button2.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MainScreenActivity));
                StartActivity(intent);
            };
            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
        }
    }
}

