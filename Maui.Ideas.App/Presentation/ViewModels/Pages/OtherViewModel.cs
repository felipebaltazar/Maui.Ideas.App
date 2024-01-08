using Maui.Ideas.App.Abstractions;
using Microsoft.Extensions.Logging;

namespace Maui.Ideas.App.Presentation.ViewModels.Pages;

public class OtherViewModel : BaseViewModel
{
    public OtherViewModel(
        ILazyDependency<ILoaderService> loaderService,
        ILazyDependency<INavigationService> navigationService,
        IMainThreadService mainThreadService,
        ILogger logger)
        : base(
            loaderService,
            navigationService,
            mainThreadService,
            logger)
    {
    }

    public async Task DoSomethingAsync()
    {
        var deviceInfo = DeviceInfo.Current;
        var manufacturer = deviceInfo.Manufacturer;

        if (deviceInfo.Platform == DevicePlatform.Android)
        {
            await NavigationService.NavigateToAsync($"androidPage?manufacturer={manufacturer}").ConfigureAwait(false);
        }
        else if (deviceInfo.Platform == DevicePlatform.iOS)
        {
            await NavigationService.NavigateToAsync($"iosPage?manufacturer={manufacturer}").ConfigureAwait(false);
        }
        else
        {
            throw new NotImplementedException("Unknow platform");
        }
    }
}
