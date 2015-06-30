using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Geolocator.Plugin.Abstractions;
using Wikitude.Demo.Model;
using WikitudeXamarinForms.Pages;
using Xamarin.Forms;

namespace WikitudeXamarinForms.Controls
{
    public class ArView : View
    {


        public TaskCompletionSource<bool> SupportedTask = new TaskCompletionSource<bool>();

        public static BindableProperty ItemsProperty = BindableProperty.Create<ArView, IEnumerable<PoiModel>>(o => o.Items, default(IEnumerable<PoiModel>));

        public IEnumerable<PoiModel> Items
        {
            get { return (IEnumerable<PoiModel>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static BindableProperty ItemClickedCommandProperty =
            BindableProperty.Create<ArView, ICommand>(x => x.ItemClickedCommand, null);

        public ICommand ItemClickedCommand
        {
            get { return (ICommand)this.GetValue(ItemClickedCommandProperty); }
            set { SetValue(ItemClickedCommandProperty, value); }
        }

        public new static readonly BindableProperty StateProperty =
            BindableProperty.Create<ArView, bool>(
                p => p.State, false);


        public new bool State
        {
            get
            {
                return (bool)GetValue(StateProperty);
            }
            set
            {
                SetValue(StateProperty, value);
            }
        }

        public new static readonly BindableProperty PositionProperty =
         BindableProperty.Create<ArView, Position>(
             p => p.Position, new Position());
        public new Position Position
        {
            get
            {
                return (Position)GetValue(PositionProperty);
            }
            set
            {
                SetValue(PositionProperty, value);
            }
        }



        public event EventHandler<String> ItemClicked;

        protected virtual void OnItemClicked(String item)
        {
            var handler = ItemClicked;
            if (handler != null) handler(this, item);
            if (ItemClickedCommand != null)
                ItemClickedCommand.Execute(item);
        }

        public void InvokeItemClicked(String s)
        {
            OnItemClicked(s);
        }

        public ArView()
        {
            HeightRequest = 200;
            WidthRequest = 20;
        }

        public void Supported(bool b)
        {
            Debug.WriteLine("Is support know: " + b);
            SupportedTask.SetResult(b);
        }
    }
}
