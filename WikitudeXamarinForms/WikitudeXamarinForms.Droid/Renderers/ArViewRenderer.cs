using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

[assembly: ExportRenderer(typeof(ArView), typeof(ArViewRenderer))]
namespace WikitudeXamarinForms.Droid.Renderers
{
    public class ArViewRenderer : ViewRenderer<ArView, ArchitectView>, ArchitectView.ISensorAccuracyChangeListener, ILocationListener, ArchitectView.IArchitectUrlListener
    {
        private ArchitectView _architectView;
        protected Location LastKnownLocation;

        protected override void OnElementChanged(ElementChangedEventArgs<ArView> e)
        {
            base.OnElementChanged(e);
            Log.Error("Wikitude", "OnElementChanged");

            if (_architectView == null)
            {
                Log.Error("Wikitude", "_architectView null ");
                _architectView = new ArchitectView(Context);
                isDestroyed = false;
                SetNativeControl(_architectView);
                var config = new StartupConfiguration(Const.WikitudeSdkKey, StartupConfiguration.Features.Geo);
                const int requiredFeatures = StartupConfiguration.Features.Geo;

                if ((ArchitectView.getSupportedFeaturesForDevice(Application.Context) & requiredFeatures) ==
                    requiredFeatures)
                {
                    _architectView.OnCreate(config);
                    _architectView.RegisterSensorAccuracyChangeListener(this);
                    _architectView.RegisterUrlListener(this);
                    _architectView.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
                    //_architectView.ScrollBarSize = 0;

                    _architectView.SetLocation(e.NewElement.Position.Latitude, e.NewElement.Position.Longitude, 10f);
                    _architectView.OnPostCreate();

                    _architectView.Load("AR/index.html");
                    if (e.NewElement.State)
                        _architectView.OnResume();
                    Element.Supported(true);
                }
                else
                {
                    _architectView = null;
                    Element.Supported(false);
                }
            }
            else
            {
                if (e.NewElement == null) return;
                if (_architectView == null) return;
                _architectView.SetLocation(e.NewElement.Position.Latitude, e.NewElement.Position.Longitude, 10f);

                if (e.NewElement.State)
                {
                    _architectView.OnResume();
                }
                else
                {
                    Destroy();
                }
            }

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            try
            {
                if (e == null) return;
                Log.Error("Wikitude", "OnElementPropertyChanged " + e.PropertyName);

                if (_architectView == null) return;
                if (e.PropertyName == ArView.StateProperty.PropertyName)
                {
                    if (Element.State)
                        _architectView.OnResume();
                    else Destroy();
                    return;
                }
                if (e.PropertyName == ArView.PositionProperty.PropertyName)
                {
                    _architectView.SetLocation(Element.Position.Latitude, Element.Position.Longitude, 10f);
                    return;
                }

                if (e.PropertyName == ArView.ItemsProperty.PropertyName)
                {
                    if (Element.Items != null && Element.Items.Any())
                    {

                        var arr = Element.Items.ToArray();
                        var json = JsonConvert.SerializeObject(arr);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            try
                            {
                                _architectView.CallJavascript("World.loadPoisFromJsonData(" + json + ")");
                            }
                            catch (Exception exception)
                            {
                                Log.Error("WikitudeTest", "Not load JS", exception);
                                Console.WriteLine(exception);
                            }
                        });
                    }
                    return;
                }
            }
            catch (Exception ee)
            {
                Log.Error("WikitudeTest", "Error ", ee);
            }
        }

       

        public bool UrlWasInvoked(string url)
        {
            var data = Tools.ParseQueryString(new Uri(url));
            if (data == null || !data.ContainsKey("id")) return false;
            Element.InvokeItemClicked(String.Format("Open {0} with id={1}", System.Net.WebUtility.UrlDecode(data["title"]), data["id"]));

            Destroy();
            return false;
        }

        private bool isDestroyed = false;
        private async void Destroy()
        {
            Log.Error("WikitudeTest", "Destroy " + isDestroyed);

            if (isDestroyed || _architectView == null) return;
            isDestroyed = true;
            _architectView.OnPause();
            _architectView.UnregisterSensorAccuracyChangeListener(this);
            _architectView.OnDestroy();
            _architectView = null;
            //await Task.Delay(500);
            Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (_architectView != null)
                {
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