using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Wikitude.Architect;
using WikitudeXamarinForms.Controls;
using WikitudeXamarinForms.Droid.Renderers;
using Application = Android.App.Application;
using Android.Util;
using PCLStorage;

[assembly: ExportRenderer(typeof(RecognizeView), typeof(RecognizeViewRenderer))]
namespace WikitudeXamarinForms.Droid.Renderers
{
    public class RecognizeViewRenderer : ViewRenderer<RecognizeView, ArchitectView>, ArchitectView.ISensorAccuracyChangeListener, ILocationListener, ArchitectView.IArchitectUrlListener
    {
        private ArchitectView _architectView;
        protected Location LastKnownLocation;

        protected override async void OnElementChanged(ElementChangedEventArgs<RecognizeView> e)
        {
            base.OnElementChanged(e);

            var config = new StartupConfiguration(Const.WikitudeSdkKey, StartupConfiguration.Features.Tracking2D);
            const int requiredFeatures = StartupConfiguration.Features.Tracking2D;

            if ((ArchitectView.getSupportedFeaturesForDevice(Application.Context) & requiredFeatures) ==
                requiredFeatures)
            {
                _architectView = new ArchitectView(Context);

                SetNativeControl(_architectView);
                _architectView.OnCreate(config);
                _architectView.RegisterUrlListener(this);
                _architectView.OnPostCreate();
                _architectView.Load("Logo/index.html");

                var path = await DownloadWTCFile();

                Log.Error("Wikitude Recognize ", "wtc path = " + path);
                if (!string.IsNullOrEmpty(path))
                    _architectView.CallJavascript("World.initWRWithFile('file:/" + path + "')");

                _architectView.OnResume();
            }
            else
            {
                _architectView = null;
                Element.OnDeviceNotSupported();
            }
        }

        private async Task<string> DownloadWTCFile()
        {
            var filename = "logos.wtc";
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync("WTC", CreationCollisionOption.OpenIfExists);
            var exist = await folder.CheckExistsAsync(filename);
            IFile file;
            if (exist == ExistenceCheckResult.FileExists)
            {
                file = await folder.GetFileAsync(filename);
                var path = file.Path;
                return path;
            }
            else
            {
                file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                using (HttpClient client = new HttpClient())
                {
                    var logosStream = await client.GetByteArrayAsync("https://www.dropbox.com/s/hc2qkgkj2tak3hg/logos.wtc?raw=1");

                    using (System.IO.Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
                    {
                        stream.Write(logosStream, 0, logosStream.Length);
                    }
                    var path = file.Path;
                    return path;
                } 
            }
            
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            try
            {
                if (_architectView == null) return;

                if (e.PropertyName == RecognizeView.WtcPathProperty.PropertyName)
                {
                    _architectView.CallJavascript("World.loadPoisFromJsonData(file://" + Element.WtcPath + ")");
                    return;
                }
            }
            catch (Exception ee)
            {
                Log.Error("WikitudeTest", "Error ", ee);
            }
        }



        public bool UrlWasInvoked(string uriString)
        {
            var invokedUri = Android.Net.Uri.Parse(uriString);
            if ("logofound".Equals(invokedUri.Host, StringComparison.InvariantCultureIgnoreCase)
               && invokedUri.GetQueryParameter("id") != null)
            {
                Toast.MakeText(Forms.Context, "Found logo: " + invokedUri.GetQueryParameter("id")+ " =\n" + BrandNames[invokedUri.GetQueryParameter("id")], ToastLength.Long).Show();
                Element.InvokeItemClicked(String.Format("Found logo: {0} \n {1}", invokedUri.GetQueryParameter("id"), BrandNames[invokedUri.GetQueryParameter("id")]));
            }
            //Element.InvokeItemClicked(String.Format("Open {0} with id={1}", System.Net.WebUtility.UrlDecode(data["title"]), data["id"]));
            return false;
        }
        private readonly Dictionary<string, string> BrandNames = new Dictionary<string, string>
        {
            {"698px-Samsung_Logo.svg_", "Samsung" },
            {"7651d831ed73796498b872af4a9f76e5.600x", "Tostitos"},
            {"kfc-funny2_20140503191959", "KFC"},
            {"Free-Google-Font-Logo-Catull-BQ-Download", "Google"},
            {"starbucks-logo", "Starbucks"}
        };
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (_architectView != null)
                {
                    _architectView.OnPause();
                    _architectView.UnregisterSensorAccuracyChangeListener(this);
                    _architectView.OnDestroy();
                }
            }
            catch (Exception e)
            {
            }

            base.Dispose(disposing);
            GC.SuppressFinalize(this);
        }

        public void OnLocationChanged(Location location)
        {
            Log.Error("WikitudeTest", "OnLocationChanged " + location);
        }

        public void OnProviderDisabled(string provider)
        {
            Log.Error("WikitudeTest", "OnProviderDisabled " + provider);
        }

        public void OnProviderEnabled(string provider)
        {
            Log.Error("WikitudeTest", "OnProviderEnabled " + provider);
        }

        public void OnStatusChanged(string provider, [GeneratedEnum]Availability status, Bundle extras)
        {
            Log.Error("WikitudeTest", "OnStatusChanged " + provider + "status = " + status);
        }

        public void OnCompassAccuracyChanged(int url)
        {
            Log.Error("WikitudeTest", "OnCompassAccuracyChanged " + url);
        }
    }
}