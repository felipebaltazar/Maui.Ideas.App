namespace Maui.Ideas.App.Abstractions;

public interface INavigationService
{
    string GetNavigationUriPath();
    Page InitializeNavigation();
    Task NavigateToAsync(string url);
    Task PushPopupAsync<T>();
}
