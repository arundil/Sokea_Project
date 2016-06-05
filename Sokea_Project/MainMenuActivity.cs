
using Android.App;
using Android.Content;
using Android.Gestures;
using Android.OS;
using Android.Util;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sokea_Project
{

    [Activity(Label = "@string/MainMenu", Icon = "@drawable/icon")]
    public class MainMenuActivity : Activity
    {
        private GestureLibrary _gestureLibrary;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //Using the view
            GestureOverlayView gestureOverlayView = new GestureOverlayView(this);
            SetContentView(gestureOverlayView);

            //Load the lobary of Gestures
            _gestureLibrary = GestureLibraries.FromRawResource(this, Resource.Raw.gestures);
            if (!_gestureLibrary.Load())
            {
                Log.Wtf(GetType().FullName, "There was a problem loading the gesture library.");
                Finish();
            }

            View view = LayoutInflater.Inflate(Resource.Layout.MainMenu, null);
            gestureOverlayView.AddView(view);
            gestureOverlayView.GesturePerformed += GestureOverlayViewOnGesturePerformed;
        }

        private void GestureOverlayViewOnGesturePerformed(object sender, GestureOverlayView.GesturePerformedEventArgs gesturePerformedEventArgs)
        {
            IEnumerable<Prediction> predictions = from p in _gestureLibrary.Recognize(gesturePerformedEventArgs.Gesture)
                                                  orderby p.Score descending
                                                  where p.Score > 1.0
                                                  select p;
            Prediction prediction = predictions.FirstOrDefault();

            if (prediction == null)
            {
                Log.Debug(GetType().FullName, "Nothing seemed to match the user's gesture, so don't do anything.");
                return;
            }

            Log.Debug(GetType().FullName, "Using the prediction named {0} with a score of {1}.", prediction.Name, prediction.Score);

            if (prediction.Name.StartsWith("checkmark"))
            {
                StartActivity(new Intent(this, typeof(CameraActivity)));
            }
            else if (prediction.Name.StartsWith("erase", StringComparison.OrdinalIgnoreCase))
            {
                // Match one of our "erase" gestures
                //_imageView.SetImageResource(Resource.Drawable.check_me);

            }
        }
    }
}