using System;
using CoreGraphics;

using Foundation;
using UIKit;

namespace WikitudeSDKExample
{
	public partial class WikitudeSDKExampleViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public WikitudeSDKExampleViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public void StartAR()
		{
			if ( !architectView.IsRunning ) {
				architectView.Start (null, null);
			}
		}

		public void StopAR()
		{
			if ( architectView.IsRunning ) {
				architectView.Stop ();
			}
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();			
			// Perform any additional setup after loading the view, typically from a nib.

			architectView.TranslatesAutoresizingMaskIntoConstraints = false;

			NSDictionary views = new NSDictionary (new NSString ("architectView"), architectView);
			this.View.AddConstraints (NSLayoutConstraint.FromVisualFormat("|-[architectView]-|", NSLayoutFormatOptions.AlignAllBaseline, null, views));
			this.View.AddConstraints (NSLayoutConstraint.FromVisualFormat("V:|-[architectView]-|", NSLayoutFormatOptions.AlignAllCenterY, null, views));

			architectView.SetLicenseKey ("");

			var path = NSBundle.MainBundle.BundleUrl.AbsoluteString + "1_ImageRecognition_1_ImageOnTarget/index.html";
			architectView.LoadArchitectWorldFromURL (NSUrl.FromString (path), Wikitude.Architect.WTFeatures.WTFeature_2DTracking);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			StartAR ();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);

			StopAR ();
		}

		#endregion

		#region Rotation

		public override void WillRotate(UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate (toInterfaceOrientation, duration);

			architectView.SetShouldRotateToInterfaceOrientation (true, toInterfaceOrientation);
		}

		public override bool ShouldAutorotate()
		{
			return true;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.All;
		}

		#endregion
	}
}

