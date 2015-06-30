
using System;
using AndroidHUD;
using WikitudeXamarinForms.Droid.Services;
using WikitudeXamarinForms.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformService))]
namespace WikitudeXamarinForms.Droid.Services
{
	public class PlatformService: IPlatformService
    {

        public void ShowLoading(string title = null, bool isBlocking = false)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (AndHUD.Shared.CurrentDialog != null)
                        AndHUD.Shared.CurrentDialog.Dismiss();
                    AndHUD.Shared.Show(Forms.Context, title, -1, isBlocking ? MaskType.Black : MaskType.None);
                }
                catch (Exception e)
                {
                    var x = e;
                }
            });
        }

        public void HideLoading()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (AndHUD.Shared.CurrentDialog != null)
                        AndHUD.Shared.CurrentDialog.Dismiss();
                }
                catch (Exception e)
                {
                    var x = e;
                }
            });
        }



    }
}