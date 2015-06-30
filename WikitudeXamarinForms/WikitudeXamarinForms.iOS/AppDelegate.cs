using System;
using System.Collections.Generic;
using System.Linq;
using CoreLocation;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace WikitudeXamarinForms.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, ICLLocationManagerDelegate
    {
        protected CoreLocation.CLLocationManager locationManager = null; 
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            // Member variable in the class
// Save a reference to the CLLocationManager so that it stays in scope



// Some method in the class
// Request geoloation authorization
locationManager = new CoreLocation.CLLocationManager();
if (locationManager.RespondsToSelector (new ObjCRuntime.Selector("requestWhenInUseAuthorization")))
{
     locationManager.RequestWhenInUseAuthorization ();
} 

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

    }
}
