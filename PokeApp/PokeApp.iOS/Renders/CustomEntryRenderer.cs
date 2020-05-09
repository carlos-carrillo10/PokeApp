using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using PokeApp.Controls;
using PokeApp.iOS.Renders;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace PokeApp.iOS.Renders
{
    public class CustomEntryRenderer  : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // set border
                Control.BorderStyle = UITextBorderStyle.Line;
            }
        }
    }
}