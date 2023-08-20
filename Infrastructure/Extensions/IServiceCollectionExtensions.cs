namespace Maui.Ideas.App.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPageAndViewModel<TPage, TViewModel>(
        this IServiceCollection serviceCollection)
        where TPage : class
        where TViewModel : class
    {

        serviceCollection.AddScoped<TPage>();
        serviceCollection.AddScoped<TViewModel>();

        return serviceCollection;
    }
}
