using Maui.Ideas.App.Abstractions;
using Maui.Ideas.App.Infrastructure.Commands;
using Maui.Ideas.App.Models;
using Microsoft.Extensions.Logging;

namespace Maui.Ideas.App.Presentation.ViewModels.Pages;

public class MainPageViewModel : BaseViewModel, IAppearingAware
{
    #region Fields

    private const string DAILY_TAB = "daily";
    private const string FAVORITES_TAB = "favorites";
    private List<Post> _fakePagination = new List<Post>(0);

    private readonly IApiRepository _apiRepository;
    private ObservableRangeCollection<Post> _postCollection = new ObservableRangeCollection<Post>();

    #endregion

    #region Properties

    public ObservableRangeCollection<Post> PostCollection
    {
        get => _postCollection;
        set => SetProperty(ref _postCollection, value);
    }

    public IAsyncCommand<string> TabSelectedCommand =>
        new AsyncCommand<string>(TabSelectedCommandExecuteAsync, (s) => !IsBusy);

    public IAsyncCommand AccountSelectCommand =>
        new AsyncCommand(AccountSelectCommandExecuteAsync, (s) => !IsBusy);

    public IAsyncCommand LoadMoreDataCommand =>
        new AsyncCommand(LoadMoreDataCommandExecuteAsync, (s) => !IsBusy);


    #endregion

    #region Constructors

    public MainPageViewModel(
        IApiRepository apiRepository,
        ILazyDependency<ILoaderService> loaderService,
        ILazyDependency<INavigationService> navigationService,
        IMainThreadService mainThreadService,
        ILogger logger)
    : base(loaderService, navigationService, mainThreadService, logger)
    {
        _apiRepository = apiRepository;
    }

    #endregion

    #region IAppearingAware

    public Task OnAppearingAsync() => LoadDailyInfoAsync();

    #endregion

    #region Private Methods

    private async Task TabSelectedCommandExecuteAsync(string selectedTab)
    {
        if (DAILY_TAB.Equals(selectedTab, StringComparison.OrdinalIgnoreCase))
        {
            await LoadDailyInfoAsync().ConfigureAwait(false);
        }
        else if (FAVORITES_TAB.Equals(selectedTab, StringComparison.OrdinalIgnoreCase))
        {
            await LoadFavoritesInfoAsync().ConfigureAwait(false);
        }
    }

    private Task AccountSelectCommandExecuteAsync() =>
         NavigationService.PushPopupAsync<ISelectAccountPopup>();

    private Task LoadMoreDataCommandExecuteAsync()
    {
        PostCollection.AddRange(_fakePagination);
        return Task.CompletedTask;
    }

    private Task LoadDailyInfoAsync()
    {
        return ExecuteBusyActionAsync(async () =>
        {
            var response = await _apiRepository.GetApiDataAsync().ConfigureAwait(false);
            _fakePagination = new List<Post>(response.Posts);

            MainThreadService.BeginInvokeOnMainThread(() => PostCollection.ReplaceRange(_fakePagination));
        });
    }

    private Task LoadFavoritesInfoAsync()
    {
        return ExecuteBusyActionAsync(() =>
        {
            var newData = new List<Post>(0);
            MainThreadService.BeginInvokeOnMainThread(() => PostCollection.ReplaceRange(newData));

            return Task.CompletedTask;
        });
    }

    #endregion
}
