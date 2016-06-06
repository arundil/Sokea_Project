using Android.App;
using Android.Views;
using Android.OS;
using Android.Graphics;
using Android.Content.PM;
using Android.Util;
using Tesseract.Droid;
using System.IO;

namespace Sokea_Project
{
    [Activity(Label = "CameraOcrActivity",Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class CameraOcrActivity : Activity, ISurfaceHolderCallback, Android.Hardware.Camera.IPreviewCallback
    {
        private bool syncObj = false;
        Android.Hardware.Camera camera;
        TesseractApi _api;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.CameraOcr);
            _api = new TesseractApi(this, AssetsDeployment.OncePerInitialization);
            await _api.Init("eng");
            SurfaceView cameraSurface = FindViewById<SurfaceView>(Resource.Id.cpPreview);
            ISurfaceHolder holder = cameraSurface.Holder;
            holder.AddCallback(this);
            holder.SetType(SurfaceType.PushBuffers);
        }

        public async void OnPreviewFrame(byte[] data, Android.Hardware.Camera camera)
        {
            if (syncObj)
                return;
            if (!_api.Initialized)
                return;
            syncObj = true;
            await _api.SetImage(ConvertYuvToJpeg(data, camera));
            var results = _api.Results(PageIteratorLevel.Block);
            foreach (var result in results)
            {
                Log.Debug("TextureViewActivity", "Word: \"{0}\", confidence: {1}", result.Text, result.Confidence);
            }
            syncObj = false;
        }

        public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
        {

        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (camera == null)
            {
                this.camera = Android.Hardware.Camera.Open();
                this.camera.SetPreviewDisplay(holder);
                this.camera.SetPreviewCallback(this);
                this.camera.StartPreview();
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {

        }

        private byte[] ConvertYuvToJpeg(byte[] yuvData, Android.Hardware.Camera camera)
        {
            var cameraParameters = camera.GetParameters();
            var width = cameraParameters.PreviewSize.Width;
            var height = cameraParameters.PreviewSize.Height;
            var yuv = new YuvImage(yuvData, cameraParameters.PreviewFormat, width, height, null);
            var ms = new MemoryStream();
            var quality = 80;   // adjust this as needed
            yuv.CompressToJpeg(new Rect(0, 0, width, height), quality, ms);
            var jpegData = ms.ToArray();

            return jpegData;
        }
    }
}