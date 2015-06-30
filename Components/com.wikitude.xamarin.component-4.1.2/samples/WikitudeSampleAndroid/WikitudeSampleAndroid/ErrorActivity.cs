
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Com.Wikitude.Samples
{
	[Activity (Label = "ErrorActivity")]			
	public class ErrorActivity : Activity
	{
		private const string TITLE = "Error";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView(Resource.Layout.activity_error);

			Title = TITLE;
		}
	}
}

