using Maui.Ideas.App.Abstractions;
using Maui.Ideas.App.Infrastructure.Extensions;

[assembly:XamlCompilation(XamlCompilationOptions.Compile)]

namespace Maui.Ideas.App;

public partial class App : Application
{
	private readonly INavigationService _navigationService;

	public App(INavigationService navigationService)
	{
        _navigationService = navigationService;

        InitializeComponent();
        MainPage = _navigationService.InitializeNavigation();
        MainPage?.RaiseOnAppearingAware();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnResume()
    {
        base.OnResume();
        MainPage?.RaiseOnAppearingAware();
    }
}
