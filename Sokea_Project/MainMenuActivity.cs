using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Android.Graphics;
using Android.Provider;
using Android.Content.PM;

namespace Sokea_Project
{
    [Activity(Label = "MainMenuActivity")]
    public class MainMenuActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainMenu);
            string text = Intent.GetStringExtra("MyData") ?? "Data not avaiable";
            string.Format(text);
            // Create your application here
        }

    }

}