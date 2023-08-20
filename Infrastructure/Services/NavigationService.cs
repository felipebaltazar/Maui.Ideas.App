using Maui.Ideas.App.Abstractions;
using Maui.Ideas.App.Infrastructure.Extensions;
using Mopups.Interfaces;
using Mopups.Pages;
using Mopups.Services;
using System.Web;

namespace Maui.Ideas.App.Infrastructure.Services;

public sealed class NavigationService : INavigationService
{
    public const string RELATIVE_URL = "/";
    public const string POP_URL = "../";
    public const string QUERY = "?";

    private readonly IServiceProvider _serviceProvider;

    private NavigationPage _rootNavigation;
    private INavigation _navigation;
    private IPopupNavigation _popupNavigation;

    public NavigationService(
        IServiceProvider provider,
        IPopupNavigation popupNavigation)
    {
        _serviceProvider = provider;

        var startPage = _serviceProvider.GetService<IStartPage>() as Page;
        ViewModelLocator.LocateViewModel(startPage, _serviceProvider);
        var navigationPage = new NavigationPage(startPage);

        _rootNavigation = navigationPage;
        _navigation = _rootNavigation.Navigation;
        _popupNavigation = popupNavigation;
    }

    public Page InitializeNavigation() => _rootNavigation;

    public string GetNavigationUriPath()
    {
        try
        {
            var pages = _navigation
                .NavigationStack
                .Select(x => x.Title ?? x.GetType().Name);


            return string.Join("/", pages);
        }
        catch (Exception)
        {
        }

        return string.Empty;

    }

    public Task NavigateToAsync(string url) =>
        NavigateToInternal(url, new Dictionary<string, string>(0), true);

    private async Task NavigateToInternal(string url, IDictionary<string, string> parameters, bool animated = true)
    {
        if (!url.StartsWith(RELATIVE_URL) &&
           !url.StartsWith(POP_URL))
        {
            ReplaceRoot(url);
            return;
        }

        var uri = new Uri(url);
        var segments = uri.Segments.Where(s => !RELATIVE_URL.Equals(s));

        foreach (var segment in segments)
        {
            var decodedSegment = HttpUtility.UrlDecode(segment);
            if (decodedSegment.Contains(".."))
            {
                await _navigation.PopAsync(animated);
                continue;
            }

            var indiceQuery = decodedSegment.IndexOf(QUERY) + 1;
            var query = indiceQuery > 0
                ? decodedSegment.Substring(indiceQuery)
                : null;

            var pagina = decodedSegment.Replace($"{QUERY}{query}", string.Empty);
            var parametros = query?
                .Split('&')
                .Select(q => q.Split('='))
                .ToDictionary(p => p[0], p => p[1]);

            parameters = parameters
                .Concat(parametros)
                .ToDictionary(p => p.Key, p => p.Value);

            await PushPageAsync(pagina, parametros, animated);
        }
    }

    private void ReplaceRoot(string url)
    {
        var uri = new Uri($"/{url}");
        var pageName = uri.Segments.First(s => !RELATIVE_URL.Equals(s));
        var page = ResolvePage(pageName);
        Application.Current.MainPage = _rootNavigation = new NavigationPage(page);
    }

    private async Task PushPageAsync(string nomePagina, IDictionary<string, string> parameters, bool animated)
    {
        var pagina = ResolvePage(nomePagina, parameters);
        await _navigation.PushAsync(pagina, animated);
    }

    private Page ResolvePage(string url, IDictionary<string, string> parametros = null)
    {
        var tipoPagina = Type.GetType($"Maui.Ideas.App.Views.Pages.{url}");
        var pagina = (Page)_serviceProvider.GetService(tipoPagina);

        ViewModelLocator.LocateViewModel(pagina, _serviceProvider);
        ParseParameters(pagina, parametros);

        return pagina;
    }

    private static void ParseParameters(Page pagina, IDictionary<string, string> parameters)
    {
        if (parameters is null)
            return;

        var vmType = pagina.BindingContext.GetType();
        foreach (var parametro in parameters)
        {
            var propriedade = vmType.GetProperty(parametro.Key);
            if (propriedade != null)
            {
                propriedade.SetValue(pagina.BindingContext, parametro.Value);
            }
        }
    }

    public async Task PushPopupAsync<T>()
    {
        var popup = _serviceProvider.GetService<T>();
        
        if (popup is PopupPage popupPage)
        {
            await _popupNavigation.PushAsync(popupPage).ConfigureAwait(false);
        }
    }
}
