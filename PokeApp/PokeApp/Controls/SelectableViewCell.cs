using Newtonsoft.Json;
using PokeApp.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PokeApp.Controls
{
    public class SelectableViewCell : ViewCell
    {
        public SelectableViewCell()
        {
            this.Tapped += CustomViewCell_Tapped;
        }

        private void CustomViewCell_Tapped(object sender, EventArgs e)
        {
            var newValue = (((ViewCell)sender).View.BackgroundColor.ToHex()).Contains("#FFFFFF") ? true : false;
            ((ViewCell)sender).View.BackgroundColor = Color.FromHex(new SelectedColorConverter().Convert(newValue, default, null, default).ToString());
           
        }

        #region Bindable Properties

        public string SelectorColor
        {
            get { return (string)GetValue(SelectorColorProperty); }
            set { SetValue(SelectorColorProperty, value); }
        }

        public static readonly BindableProperty SelectorColorProperty =
        BindableProperty.Create("SelectorColor", typeof(string), typeof(string), propertyChanged: SelectorColorChanged);

        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
            set
            {
                SetValue(SelectorColorProperty, value);
                OnPropertyChanged(nameof(Selected));
            }
        }

        public static readonly BindableProperty SelectedProperty =
        BindableProperty.Create("SelectedColor", typeof(bool), typeof(bool), default, BindingMode.TwoWay, propertyChanged: SelectedChanged);

        #endregion

        #region Methods

        private static void SelectorColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
          
        }

        private static void SelectedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null)
                ((ViewCell)bindable).View.BackgroundColor = Color.FromHex(new SelectedColorConverter().Convert(newValue, default, null, default).ToString());
        }
        #endregion

    }
}
