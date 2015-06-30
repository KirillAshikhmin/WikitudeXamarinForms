using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Size = Xamarin.Forms.Size;
using System.Collections.Generic;
using System.Linq;
using BigTed;
using WikitudeXamarinForms.iOS.Services;
using WikitudeXamarinForms.Services;

[assembly: Dependency(typeof(PlatformService))]
namespace WikitudeXamarinForms.iOS.Services
{
	public class PlatformService: IPlatformService
    {
		
        public void ShowLoading(string title = null, bool isBlocking = false)
        {
            Device.BeginInvokeOnMainThread(() => BTProgressHUD.Show(title, -1F, isBlocking?ProgressHUD.MaskType.Black : ProgressHUD.MaskType.None));
        }

        public void HideLoading()
        {
            Device.BeginInvokeOnMainThread(BTProgressHUD.Dismiss);
        }


    }
}