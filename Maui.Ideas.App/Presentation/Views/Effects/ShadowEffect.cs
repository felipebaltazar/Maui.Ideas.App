
using Microsoft.Maui.Controls.Platform;
using System.ComponentModel;
using static Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.VisualElement;
using Color = Microsoft.Maui.Graphics.Color;
using Microsoft.Maui.Platform;


#if __ANDROID__
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using AButton = Android.Widget.Button;
using AEditText = Android.Widget.EditText;
using ATextView = Android.Widget.TextView;
using AView = Android.Views.View;

#elif __IOS__
using NativeView = UIKit.UIView;
using CoreGraphics;
#endif

namespace Maui.Ideas.App.Presentation.Views.Effects;

public class ShadowRoutingEffect : RoutingEffect
{
    internal const string ColorPropertyName = "Color";

    internal const string OpacityPropertyName = "Opacity";

    internal const string RadiusPropertyName = "Radius";

    internal const string OffsetXPropertyName = "OffsetX";

    internal const string OffsetYPropertyName = "OffsetY";

    public static readonly BindableProperty ColorProperty = BindableProperty.CreateAttached(
        ColorPropertyName,
        typeof(Color),
        typeof(ShadowEffect),
        Colors.White,
        propertyChanged: TryGenerateEffect);

    public static readonly BindableProperty OpacityProperty = BindableProperty.CreateAttached(
        OpacityPropertyName,
        typeof(float),
        typeof(ShadowEffect),
        -1.0f,
        propertyChanged: TryGenerateEffect);

    public static readonly BindableProperty RadiusProperty = BindableProperty.CreateAttached(
        RadiusPropertyName,
        typeof(double),
        typeof(ShadowEffect),
        -1.0,
        propertyChanged: TryGenerateEffect);

    public static readonly BindableProperty OffsetXProperty = BindableProperty.CreateAttached(
        OffsetXPropertyName,
        typeof(double),
        typeof(ShadowEffect),
        .0,
        propertyChanged: TryGenerateEffect);

    public static readonly BindableProperty OffsetYProperty = BindableProperty.CreateAttached(
        OffsetYPropertyName,
        typeof(double),
        typeof(ShadowEffect),
        .0,
        propertyChanged: TryGenerateEffect);

    public static Color GetColor(BindableObject bindable)
        => (Color)bindable.GetValue(ColorProperty);

    public static void SetColor(BindableObject bindable, Color value)
        => bindable.SetValue(ColorProperty, value);

    public static float GetOpacity(BindableObject bindable)
        => (float)bindable.GetValue(OpacityProperty);

    public static void SetOpacity(BindableObject bindable, double value)
        => bindable.SetValue(OpacityProperty, value);

    public static double GetRadius(BindableObject bindable)
        => (double)bindable.GetValue(RadiusProperty);

    public static void SetRadius(BindableObject bindable, double value)
        => bindable.SetValue(RadiusProperty, value);

    public static double GetOffsetX(BindableObject bindable)
        => (double)bindable.GetValue(OffsetXProperty);

    public static void SetOffsetX(BindableObject bindable, double value)
        => bindable.SetValue(OffsetXProperty, value);

    public static double GetOffsetY(BindableObject bindable)
        => (double)bindable.GetValue(OffsetYProperty);

    public static void SetOffsetY(BindableObject bindable, double value)
        => bindable.SetValue(OffsetYProperty, value);

    static void TryGenerateEffect(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is VisualElement view))
            return;

        var shadowEffects = view.Effects.OfType<ShadowEffect>();

        if (GetColor(view) == Colors.White)
        {
            foreach (var effect in shadowEffects.ToArray())
                view.Effects.Remove(effect);

            return;
        }

        if (!shadowEffects.Any())
            view.Effects.Add(new ShadowEffect());
    }
}

#if ANDROID

internal class ShadowPlatformEffect : PlatformEffect
{
    const float defaultRadius = 10f;

    const float defaultOpacity = .8f;

    AView View => Control ?? Container;

    protected override void OnAttached()
        => Update();

    protected override void OnDetached()
    {
        if (View == null)
            return;

        View.Elevation = 0;
    }

    protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnElementPropertyChanged(args);

        if (View == null)
            return;

        switch (args.PropertyName)
        {
            case ShadowRoutingEffect.ColorPropertyName:
            case ShadowRoutingEffect.OpacityPropertyName:
            case ShadowRoutingEffect.RadiusPropertyName:
            case ShadowRoutingEffect.OffsetXPropertyName:
            case ShadowRoutingEffect.OffsetYPropertyName:
            case nameof(VisualElement.Width):
            case nameof(VisualElement.Height):
            case nameof(VisualElement.BackgroundColor):
            case nameof(IBorderElement.CornerRadius):
                View.Invalidate();
                Update();
                break;
        }
    }

    void Update()
    {
        if (View == null || Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            return;

        var radius = (float)ShadowRoutingEffect.GetRadius(Element);
        if (radius < 0)
            radius = defaultRadius;

        var opacity = ShadowRoutingEffect.GetOpacity(Element);
        if (opacity < 0)
            opacity = defaultOpacity;

        var color = ShadowRoutingEffect.GetColor(Element);
        color = color.MultiplyAlpha(opacity);

        var androidColor = color.ToAndroid();
        var offsetX = (float)ShadowRoutingEffect.GetOffsetX(Element);
        var offsetY = (float)ShadowRoutingEffect.GetOffsetY(Element);
        var cornerRadius = Element is IBorderElement borderElement ? borderElement.CornerRadius : 0;

        if (View is AButton button)
        {
            button.StateListAnimator = null;
        }
        else if (View is not AEditText && View is ATextView textView)
        {
            textView.SetShadowLayer(radius, offsetX, offsetY, androidColor);
            return;
        }

        var pixelOffsetX = View.Context.ToPixels(offsetX);
        var pixelOffsetY = View.Context.ToPixels(offsetY);
        var pixelCornerRadius = View.Context.ToPixels(cornerRadius);
        View.OutlineProvider = new ShadowOutlineProvider(pixelOffsetX, pixelOffsetY, pixelCornerRadius);
        View.Elevation = View.Context.ToPixels(radius);
        if (View.Parent is ViewGroup group)
            group.SetClipToPadding(false);

#pragma warning disable
        if (Build.VERSION.SdkInt < BuildVersionCodes.P)
            return;

        View.SetOutlineAmbientShadowColor(androidColor);
        View.SetOutlineSpotShadowColor(androidColor);
#pragma warning restore
    }

    class ShadowOutlineProvider : ViewOutlineProvider
    {
        readonly float offsetX;
        readonly float offsetY;
        readonly float cornerRadius;

        public ShadowOutlineProvider(float offsetX, float offsetY, float cornerRadius)
        {
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.cornerRadius = cornerRadius;
        }

        public override void GetOutline(AView? view, Outline? outline)
            => outline?.SetRoundRect((int)offsetX, (int)offsetY, view?.Width ?? 0, view?.Height ?? 0, cornerRadius);
    }
}
#elif IOS

public class ShadowPlatformEffect : PlatformEffect
{
    const float defaultRadius = 10f;

    const float defaultOpacity = .5f;

    NativeView View
    {
        get
        {
            var view = Control ?? Container;
            return Element is Frame ? view?.Subviews.FirstOrDefault() ?? view : view;
        }
    }

    protected override void OnAttached()
    {
        if (View == null)
            return;

        Update(View);
    }

    protected override void OnDetached()
    {
        if (View?.Layer == null)
            return;

        View.Layer.ShadowOpacity = 0;
    }

    protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnElementPropertyChanged(args);

        if (View == null)
            return;

        switch (args.PropertyName)
        {
            case ShadowRoutingEffect.ColorPropertyName:
                UpdateColor(View);
                break;
            case ShadowRoutingEffect.OpacityPropertyName:
                UpdateOpacity(View);
                break;
            case ShadowRoutingEffect.RadiusPropertyName:
                UpdateRadius(View);
                break;
            case ShadowRoutingEffect.OffsetXPropertyName:
            case ShadowRoutingEffect.OffsetYPropertyName:
                UpdateOffset(View);
                break;
            case nameof(VisualElement.Width):
            case nameof(VisualElement.Height):
            case nameof(VisualElement.BackgroundColor):
            case nameof(IBorderElement.CornerRadius):
                Update(View);
                break;
        }
    }

    void UpdateColor(in NativeView view)
    {
        if (view.Layer != null)
            view.Layer.ShadowColor = ShadowRoutingEffect.GetColor(Element).ToCGColor();
    }

    void UpdateOpacity(in NativeView view)
    {
        if (view.Layer != null)
        {
            var opacity = ShadowRoutingEffect.GetOpacity(Element);
            view.Layer.ShadowOpacity = opacity < 0 ? defaultOpacity : opacity;
        }
    }

    void UpdateRadius(in NativeView view)
    {
        if (view.Layer != null)
        {
            var radius = (nfloat)ShadowRoutingEffect.GetRadius(Element);
            view.Layer.ShadowRadius = radius < 0 ? defaultRadius : radius;
        }
    }

    void UpdateOffset(in NativeView view)
    {
        if (view.Layer != null)
            view.Layer.ShadowOffset = new CGSize((double)ShadowRoutingEffect.GetOffsetX(Element), (double)ShadowRoutingEffect.GetOffsetY(Element));
    }

    void Update(in NativeView view)
    {
        UpdateColor(view);
        UpdateOpacity(view);
        UpdateRadius(view);
        UpdateOffset(view);
    }
}
#endif
