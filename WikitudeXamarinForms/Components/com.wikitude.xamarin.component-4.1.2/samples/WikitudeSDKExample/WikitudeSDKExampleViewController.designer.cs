// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;
using Wikitude.Architect;


namespace WikitudeSDKExample
{
	[Register ("WikitudeSDKExampleViewController")]
	partial class WikitudeSDKExampleViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		Wikitude.Architect.WTArchitectView architectView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (architectView != null) {
				architectView.Dispose ();
				architectView = null;
			}
		}
	}
}
