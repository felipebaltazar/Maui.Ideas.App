using Mopups.Pages;

namespace Maui.Ideas.App.Presentation.Views.Controls;

public partial class LoaderPopup : PopupPage
{
	public LoaderPopup()
	{
		InitializeComponent();
	}

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}