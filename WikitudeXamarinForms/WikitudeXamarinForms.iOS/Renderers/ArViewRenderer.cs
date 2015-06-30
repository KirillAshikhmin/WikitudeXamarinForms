using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Foundation;
using Newtonsoft.Json;
using UIKit;
using Wikitude.Architect;
using WikitudeXamarinForms.Controls;
using WikitudeXamarinForms.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#if !__SIMULATOR__
[assembly: ExportRenderer(typeof(ArView), typeof(ArViewRenderer))]
#endif
namespace WikitudeXamarinForms.iOS.Renderers
{
    public class ArViewRenderer : ViewRenderer<ArView, WTArchitectView>
    {
        private WTArchitectView _architectView;

        protected override void OnElementChanged(ElementChangedEventArgs<ArView> e)
        {
            base.OnElementChanged(e);

            if (Control != null) return;

            NSError err;
            if (WTArchitectView.IsDeviceSupportedForRequiredFeatures(WTFeatures.Geo, out err))
            {
                _architectView = new WTArchitectView();
              
                _architectView.SetLicenseKey(licenseKey: Const.WikitudeSdkKey);

                var path = NSBundle.MainBundle.BundleUrl.AbsoluteString + "AR/index.html";
                _architectView.LoadArchitectWorldFromURL(NSUrl.FromString(path), WTFeatures.Geo);
               // _architectView.LoadArchitectWorldFromURL(NSUrl.FromString("http://wikitude.world.url.com"), WTFeatures.WTFeature_2DTracking);

				SetNativeControl(_architectView);

                _architectView.Start(null, null);
            }
            if (err != null)
            {
                Console.WriteLine(err);
                //Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", err.LocalizedDescription, "Ok");
                var adErr = new UIAlertView("Unsupported Device", "This device is not capable of running ARchitect Worlds. Requirements are: iOS 5 or higher, iPhone 3GS or higher, iPad 2 or higher. Note: iPod Touch 4th and 5th generation are only supported in WTARMode_IR.", null, "OK", null);
                adErr.Show();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (_architectView == null) return;

            if (e.PropertyName == ArView.ItemsProperty.PropertyName)
            {
                if (Element.Items != null && Element.Items.Any())
                {
                    /*
                    var rnd = new Random();

                    foreach (var poiModel in Element.Items)
                    {
                        poiModel.Latitude = 51.7279464 + rnd.NextDouble()/5 - 0.1;
                        poiModel.Longitude = 39.2294512 + rnd.NextDouble()/5 - 0.1;
                    }
                    */
                    var arr = Element.Items.ToArray();
                    var json = JsonConvert.SerializeObject(arr);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            _architectView.CallJavaScript("World.loadPoisFromJsonData(" + json + ")");

							foreach (var v in _architectView.Subviews){
								var webView = v as UIWebView;
								if (webView == null) continue;
								webView.Delegate = new WTWebViewDelegate(Element);				
							}
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                        }
                    });
                }
            }
        }

        public void Stop()
        {
            if (_architectView != null && _architectView.IsRunning)
                _architectView.Stop();
        }

        protected override void Dispose(bool disposing)
        {
            Stop();
            base.Dispose(disposing);
        }
    }

	public class WTWebViewDelegate: UIWebViewDelegate{

		private readonly ArView _arView;

	    public WTWebViewDelegate (ArView arView)
		{
			_arView = arView;
		}

		public override bool ShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
		{
			if (request.Url.Scheme != "architectsdk" || request.Url.Host != "markerselected") return false;

			var data = Tools.ParseQueryString(new Uri(request.Url.ToString()));
                if (data == null || !data.ContainsKey("id")) return false;
            /*
                if (data["type"] == "House")
                    _arView.InvokeItemClicked(new House { Id = data["id"] });
                else
                    _arView.InvokeItemClicked(new Museum { Id = data["id"] });
            */
			return false;
		}
	}
}