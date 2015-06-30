using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Wikitude.Demo.Model;
using WikitudeXamarinForms.Pages;
using Xamarin.Forms;

namespace WikitudeXamarinForms
{
    public class App : Application
    {
        public static List<PoiModel> Points;

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            Debug.WriteLine("OnChildAdded " + child.ToString());

        }

        protected override void OnChildRemoved(Element child)
        {
            base.OnChildRemoved(child);
            Debug.WriteLine("OnChildRemoved " + child.ToString());
        }

        public App()
        {
            // The root page of your application
            Points = new List<PoiModel>
            {
                new PoiModel(){Id = Guid.NewGuid().ToString("N"), Type = PoiTypes.Museum.ToString(), Name = "Церковь на Лазаревском кладбище", Description = "description"},
                new PoiModel(){Id = Guid.NewGuid().ToString("N"), Type = PoiTypes.House.ToString(), Name = "Гостиница и ресторан \"Эльдорадо\"", Description = "description"},
                new PoiModel(){Id = Guid.NewGuid().ToString("N"), Type = PoiTypes.House.ToString(), Name = "Флигель усадьбы Раевских", Description = "description"},
                new PoiModel(){Id = Guid.NewGuid().ToString("N"), Type = PoiTypes.House.ToString(), Name = "Ещё какойто домик", Description = "description"},
                new PoiModel(){Id = Guid.NewGuid().ToString("N"), Type = PoiTypes.Museum.ToString(), Name = "Музей имени меня любимого", Description = "description"},
                new PoiModel(){Id = Guid.NewGuid().ToString("N"), Type = PoiTypes.House.ToString(), Name = "Дом отрочества Абамы", Description = "description"},
                new PoiModel(){Id = Guid.NewGuid().ToString("N"), Type = PoiTypes.House.ToString(), Name = "Алибаба и сорок разбойников", Description = "description"},
                new PoiModel(){Id = Guid.NewGuid().ToString("N"), Type = PoiTypes.House.ToString(), Name = "Ресторан на холме", Description = "description"},
                new PoiModel(){Id = Guid.NewGuid().ToString("N"), Type = PoiTypes.Museum.ToString(), Name = "Забыл как называется", Description = "description"},
                new PoiModel(){Id = Guid.NewGuid().ToString("N"), Type = PoiTypes.Museum.ToString(), Name = "Политика украины в каменном веке", Description = "description"},
            };

            MainPage = new NavigationPage(new ContentPage
            {
                
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
						new Label {
							XAlign = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						},
                        new Button
                        {
                            Text = "Open  Wikitude",
                            Command =  new Command (()=>((NavigationPage)MainPage).PushAsync(new ArPage(Points)))
                        }
					}
                }
            });

            ((NavigationPage)MainPage).Pushed += delegate(object sender, NavigationEventArgs args)
            {
                Debug.WriteLine("Pushed " );
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
