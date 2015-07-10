using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Foundation;
using Newtonsoft.Json;
using PCLStorage;
using UIKit;
using Wikitude.Architect;
using WikitudeXamarinForms.Controls;
using WikitudeXamarinForms.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

#if !__SIMULATOR__
[assembly: ExportRenderer(typeof(RecognizeView), typeof(RecognizeViewRenderer))]
#endif
namespace WikitudeXamarinForms.iOS.Renderers
{
    public class RecognizeViewRenderer : ViewRenderer<RecognizeView, WTArchitectView>
    {
        private WTArchitectView _architectView;
        private string wtcPath;

        protected override async void OnElementChanged(ElementChangedEventArgs<RecognizeView> e)
        {
            base.OnElementChanged(e);

            if (Control != null) return;

            NSError err;
            bool s = WTArchitectView.IsDeviceSupportedForRequiredFeatures(WTFeatures.WTFeature_2DTracking, out err);
            Debug.WriteLine("Support: " + s);
            _architectView = new WTArchitectView();

            _architectView.SetLicenseKey(Const.WikitudeSdkKey);

            var path = NSBundle.MainBundle.BundleUrl.AbsoluteString + "Logo/index.html";
            _architectView.LoadArchitectWorldFromURL(NSUrl.FromString(path), WTFeatures.WTFeature_2DTracking);


            SetNativeControl(_architectView);

            /*
                            wtcPath = await DownloadWTCFile();

                            Debug.WriteLine("wtc path =  file:///private" + wtcPath );

                        if (!string.IsNullOrEmpty(wtcPath))
                        {
                            wtcPath = "file:///private" + wtcPath;
                            _architectView.CallJavaScript("World.loadPoisFromJsonData('"+wtcPath+"')");
                            _architectView.CallJavaScript("World.initWithFile('" + wtcPath + "')");
                        }
                        */

            _architectView.Start(null, null);
            foreach (var v in _architectView.Subviews)
            {
                var webView = v as UIWebView;
                if (webView == null) continue;
                webView.Delegate = new WTWebRecognizeViewDelegate(e.NewElement);
            }
            /*
            if (err != null)
            {
                Console.WriteLine(err);
                //Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", err.LocalizedDescription, "Ok");
                var adErr = new UIAlertView("Unsupported Device", "This device is not capable of running ARchitect Worlds. Requirements are: iOS 5 or higher, iPhone 3GS or higher, iPad 2 or higher. Note: iPod Touch 4th and 5th generation are only supported in WTARMode_IR.", null, "OK", null);
                adErr.Show();
            }*/

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

            if (_architectView == null) return;


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

    public class WTWebRecognizeViewDelegate : UIWebViewDelegate
    {

        private readonly RecognizeView _recognizeView;

        public WTWebRecognizeViewDelegate(RecognizeView recognizeView)
        {
            _recognizeView = recognizeView;
        }

        public override bool ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            if (request.Url.Scheme != "architectsdk" || request.Url.Host != "markerselected") return false;

            var data = Tools.ParseQueryString(new Uri(request.Url.ToString()));
            if (data == null || !data.ContainsKey("id")) return false;
            _recognizeView.InvokeItemClicked(String.Format("Found logo: {0} \n {1}", data["id"], BrandNames[data["id"]]));


            /*
                if (data["type"] == "House")
                    _arView.InvokeItemClicked(new House { Id = data["id"] });
                else
                    _arView.InvokeItemClicked(new Museum { Id = data["id"] });
            */
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
    }
}