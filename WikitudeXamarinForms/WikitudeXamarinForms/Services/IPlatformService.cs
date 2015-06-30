using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wikitude.Demo.Model;

namespace WikitudeXamarinForms.Services
{
    public interface IPlatformService
    {
        void ShowLoading(string title = null, bool isBlocking = false);
        void HideLoading();
     //   void SelectItem(string itemId);

     //   event EventHandler<string> ArItemSelected;
    }
}
