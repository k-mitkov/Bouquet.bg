﻿using System.ComponentModel;
using Android.Content;
using Android.Views.Accessibility;
using Java.Lang;
using Bouquet.Mobile.CustomControls;
using Bouquet.Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CustomSwitchRenderer))]

namespace Bouquet.Mobile.Droid.Renderers
{
    public sealed class CustomSwitchRenderer : VisualElementRenderer<ContentView>
    {
        private readonly Android.Widget.Switch _a11YSwitch;
        public CustomSwitchRenderer(Context context) : base(context)
        {
            _a11YSwitch = new Android.Widget.Switch(context);
        }

        /// <inheritdoc />
        protected override void OnElementChanged(ElementChangedEventArgs<ContentView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            CustomSwitch customSwitch = e.NewElement as CustomSwitch;

            _a11YSwitch.Checked = customSwitch.IsToggled;

        }

        /// <inheritdoc />
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals(nameof(CustomSwitch.IsToggled)))
            {
                CustomSwitch customSwitch = sender as CustomSwitch;

                if (_a11YSwitch.Checked != customSwitch.IsToggled)
                {
                    _a11YSwitch.Checked = customSwitch.IsToggled;
                    AnnounceForAccessibility(GetStateDescription());
                }
            }
        }

        /// <inheritdoc />
        public override ICharSequence? AccessibilityClassNameFormatted => new String(_a11YSwitch.Class.Name);

        /// <inheritdoc />
        public override void OnInitializeAccessibilityNodeInfo(AccessibilityNodeInfo? info)
        {
            if (info != null)
            {
                info.Checkable = true;
                info.Checked = _a11YSwitch.Checked;
                info.Text = GetStateDescription();
            }

            base.OnInitializeAccessibilityNodeInfo(info);
        }


        private string GetStateDescription()
        {
            return _a11YSwitch.Checked ? _a11YSwitch.TextOn : _a11YSwitch.TextOff;
        }
    }
}