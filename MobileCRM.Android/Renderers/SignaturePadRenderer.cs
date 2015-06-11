using System;
using Android.Views;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using MobileCRMAndroid.Renderers;
using System.ComponentModel;
using SignaturePad;

[assembly: ExportRenderer(typeof(MobileCRM.CustomControls.SignaturePad), typeof(SignaturePadRenderer))]

namespace MobileCRMAndroid.Renderers
{
    public class SignaturePadRenderer : ViewRenderer<MobileCRM.CustomControls.SignaturePad, SignaturePadView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<MobileCRM.CustomControls.SignaturePad> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
                return;

            var signaturePad = new SignaturePadView(Forms.Context)
            {
                BackgroundColor = this.Element.BackgroundColor.ToAndroid(),
                StrokeColor = Element.StrokeColor.ToAndroid(),
                SignatureLineColor = Element.SignatureLineColor.ToAndroid(),
        
            };

            signaturePad.SignaturePrompt.Text = Element.SignaturePrompt;
            signaturePad.SignaturePrompt.SetTextColor(Element.SignaturePromptColor.ToAndroid());
            signaturePad.Caption.Text = Element.Caption;
            signaturePad.Caption.SetTextColor(Element.CaptionColor.ToAndroid());

            if (!string.IsNullOrWhiteSpace(Element.DefaultPoints))
            {
                try
                {
                    signaturePad.LoadPoints(Newtonsoft.Json.JsonConvert.DeserializeObject<System.Drawing.PointF[]>(Element.DefaultPoints));
                }
                catch (Exception ex)
                {

                }
            }

            Element.GetPointString = () =>
            {
                if (signaturePad.Points == null)
                    return string.Empty;

                return Newtonsoft.Json.JsonConvert.SerializeObject(signaturePad.Points);
            };

            this.Element.GetImageEvent += Element_GetImageEvent;

            ((Android.Widget.RelativeLayout)signaturePad).LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.WrapContent);
            SetNativeControl(signaturePad);
        }

        void Element_GetImageEvent(object sender, EventArgs e)
        {
            //save image here?
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
                return;
        }
    }
}