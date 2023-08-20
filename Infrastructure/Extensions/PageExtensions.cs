using Maui.Ideas.App.Abstractions;

namespace Maui.Ideas.App.Infrastructure.Extensions;

public static class PageExtensions
{
    public static void RaiseOnAppearingAware(this Page page)
    {
        if (page is NavigationPage nav)
        {
            RaiseOnAppearingAware(nav.CurrentPage);
            return;
        }

        if (page?.BindingContext is not IAppearingAware appearingAware)
            return;

        _ = Task.Run(appearingAware.OnAppearingAsync);
    }
}
