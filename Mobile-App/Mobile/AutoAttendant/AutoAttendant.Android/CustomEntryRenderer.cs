using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using AutoAttendant;
using AutoAttendant.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRendererAndroid))]
namespace AutoAttendant.Droid
{
    public class CustomEntryRendererAndroid : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(Android.Graphics.Color.Transparent);
                this.Control.SetBackground(gd);
                this.Control.SetPadding(20, 0, 0, 0);

                CustomEntry customEntry = (CustomEntry)e.NewElement;
                if (!customEntry.IsPasswordFlag)
                {
                    this.Control.InputType = InputTypes.TextVariationVisiblePassword;
                }

            }

        }
        public CustomEntryRendererAndroid(Context context) : base(context)
        {
        }
    }
}