using System;
using System.Collections.Generic;
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
          //SelectorColor ="#000000";
        }

        #region Bindable Properties

        public string SelectorColor
        {
            get { return (string)GetValue(SelectorColorProperty); }
            set { SetValue(SelectorColorProperty, value); }
        }

        public static readonly BindableProperty SelectorColorProperty =
        BindableProperty.Create("SelectorColor", typeof(string), typeof(string), propertyChanged: SelectorColorChanged);
        #endregion

        #region Methods

        private static void SelectorColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
          //  ((ViewCell)bindable).View.BackgroundColor = Color.FromHex((string)newValue);
        }
        #endregion

    }
}
