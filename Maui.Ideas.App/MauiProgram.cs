using Maui.Ideas.App.Abstractions;
using Maui.Ideas.App.Data;
using Maui.Ideas.App.Infrastructure;
using Maui.Ideas.App.Infrastructure.Extensions;
using Maui.Ideas.App.Infrastructure.Services;
using Maui.Ideas.App.Models;
using Maui.Ideas.App.Presentation.ViewModels.Pages;
using Maui.Ideas.App.Presentation.Views.Controls;
using Maui.Ideas.App.Presentation.Views.Effects;
using Maui.Ideas.App.Presentation.Views.Pages;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Mopups.Interfaces;
using Mopups.Services;
using Refit;
using System.Reflection.Metadata;

namespace Maui.Ideas.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureEffects(e =>
            {
#if __ANDROID__ || __IOS__
                e.Add<ShadowRoutingEffect, ShadowPlatformEffect>();
#endif
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Lato-Black.ttf", "LatoBlack");
                fonts.AddFont("Lato-Bold.ttf", "LatoBold");
                fonts.AddFont("Lato-Light.ttf", "LatoLight");
                fonts.AddFont("Lato-Regular.ttf", "LatoRegular");
            })
            .ConfigureMopups();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        var apiData = RestService.For<IApiData>(Constants.Api.BASE_URL);

        //Register Services
        builder.Services.AddSingleton(apiData);
        builder.Services.AddSingleton<IApiRepository, ApiRepository>();
        builder.Services.AddSingleton<ILogger, AppCenterLoggerService>();
        builder.Services.AddSingleton<IDeviceInfoService, DeviceInfoService>();
        builder.Services.AddSingleton<IMainThreadService, MainThreadService>();
        builder.Services.AddSingleton(MopupService.Instance);

        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<ILazyDependency<INavigationService>, LazyDependency<INavigationService>>();

        builder.Services.AddSingleton<ILoaderService, LoaderService>();
        builder.Services.AddSingleton<ILazyDependency<ILoaderService>, LazyDependency<ILoaderService>>();

        //Register ViewModels and Views
        builder.Services.AddScoped<MainPageViewModel>();
        builder.Services.AddScoped<IStartPage, MainPage>();
        builder.Services.AddScoped<ISelectAccountPopup, SelectAccountPopup>();

        return builder.Build();
    }
}
