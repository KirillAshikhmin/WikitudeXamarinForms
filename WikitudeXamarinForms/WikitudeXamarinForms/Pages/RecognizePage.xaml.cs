using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Geolocator.Plugin;
using PCLStorage;
using Wikitude.Demo.Model;
using WikitudeXamarinForms.Controls;
using WikitudeXamarinForms.Services;
using Xamarin.Forms;
using Position = Geolocator.Plugin.Abstractions.Position;

namespace WikitudeXamarinForms.Pages
{
    public partial class RecognizePage : ContentPage
    {
        private bool _initDone;

        public RecognizePage()
        {
            InitializeComponent();

            Title = "Recognize";
            _initDone = false;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!_initDone)
                await Task.Run(() => InitPage());

        }

        protected override void OnDisappearing()
        {
            DependencyService.Get<IPlatformService>().HideLoading();
            base.OnDisappearing();
        }

        private void InitPage()
        {
            _initDone = true;

            DependencyService.Get<IPlatformService>().ShowLoading("Recognize init");

            RecognizeViewer.ItemClickedCommand = new Command(ArItemClickedCommandExecute);

            Debug.WriteLine("Page Init");
            DependencyService.Get<IPlatformService>().HideLoading();
        }

     

        private void ArItemClickedCommandExecute(object o)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Debug.WriteLine("Clicked");
                Navigation.PushAsync(new ContentPage
                {
                    Title = "Main in stack",

                    Content = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Children =
                    {
                        new Label
                        {
                            XAlign = TextAlignment.Center,
                            Text = o.ToString()
                        }
                    }
                    }
                });

            }); 
        }

        private async void OnDeviceNotSupported(object sender, EventArgs e)
        {
            await Application.Current.MainPage.DisplayAlert("Localization.Error_Title",
                "Localization.Error_AR_Unsupported",
                "Localization.Error_Btn_Ok");

            await Navigation.PopAsync(true);
        }
    }
}
