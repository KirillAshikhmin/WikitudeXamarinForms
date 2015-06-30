using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Util;
using Java.IO;
using Wikitude.Architect;
using Android.Locations;

namespace Com.Wikitude.Samples
{
	[Activity (Label = "Wikitude Samples", MainLauncher = true)]
	public class MainActivity : Activity 
	{
		protected ArchitectView architectView;

		private const string SAMPLE_WORLD_URL = "samples/1_Image$Recognition_1_Image$On$Target/index.html";

		private const string TITLE = "Test World";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView(Resource.Layout.sample_cam);

			Title = TITLE;

			architectView = FindViewById<ArchitectView>(Resource.Id.architectView);
			var config = new StartupConfiguration (Constants.WIKITUDE_SDK_KEY, StartupConfiguration.Features.Tracking2D);
			/* use  
			   int requiredFeatures = StartupConfiguration.Features.Tracking2D | StartupConfiguration.Features.Geo;
			   if you need both 2d Tracking and Geo
			*/
			int requiredFeatures = StartupConfiguration.Features.Tracking2D | StartupConfiguration.Features.Geo;
			if ((ArchitectView.getSupportedFeaturesForDevice (Android.App.Application.Context) & requiredFeatures) == requiredFeatures) {
				architectView.OnCreate (config);
			} else {
				architectView = null;
				StartActivity (typeof(ErrorActivity));
			}				
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			if (architectView != null)
				architectView.OnResume ();
		}

		protected override void OnPause ()
		{
			base.OnPause ();

			if (architectView != null)
				architectView.OnPause ();
		}

		protected override void OnStop ()
		{
			base.OnStop ();
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			if (architectView != null)
			{
				architectView.OnDestroy ();
			}
		}

		public override void OnLowMemory ()
		{
			base.OnLowMemory ();

			if (architectView != null)
				architectView.OnLowMemory ();
		}

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);

			if (architectView != null) {
				architectView.OnPostCreate ();

				try {
					architectView.Load (SAMPLE_WORLD_URL);
				} catch (Exception ex) {
					Log.Error ("WIKITUDE_SAMPLE", ex.ToString ());
				}
			}
		}
	}
}
