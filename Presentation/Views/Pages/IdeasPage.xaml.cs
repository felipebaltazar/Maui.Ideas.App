namespace Maui.Ideas.App.Presentation.Views.Pages;

public partial class IdeasPage : ContentPage
{
	public IdeasPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = floatingButton.TranslateTo(0, 0, 300, Easing.BounceIn);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		_ = floatingButton.TranslateTo(0, 80, 300, Easing.BounceOut);
    }
}