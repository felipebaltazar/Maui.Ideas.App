using FluentAssertions;
using Maui.Ideas.App.Abstractions;
using Maui.Ideas.App.Models;
using Maui.Ideas.App.Presentation.ViewModels.Pages;
using Microsoft.Extensions.Logging;
using Moq;

namespace Maui.Ideas.App.Tests;

public sealed class MainPageViewModelTests
{
    #region Mock 

    private readonly Mock<IApiRepository> _apiRepository =
        new Mock<IApiRepository>();

    private readonly Mock<ILoaderService> _loaderService =
        new Mock<ILoaderService>();

    private readonly Mock<INavigationService> _navigationService =
        new Mock<INavigationService>();

    private readonly Mock<ILazyDependency<ILoaderService>> _lazyLoaderService =
        new Mock<ILazyDependency<ILoaderService>>();

    private readonly Mock<ILazyDependency<INavigationService>> _lazyNavigationService =
        new Mock<ILazyDependency<INavigationService>>();

    private readonly Mock<IMainThreadService> _mainThreadService =
        new Mock<IMainThreadService>();

    private readonly Mock<ILogger> _logger =
        new Mock<ILogger>();

    #endregion

    private readonly MainPageViewModel ViewModel;

    public MainPageViewModelTests()
    {
        _lazyLoaderService.SetupGet(l => l.Value)
                          .Returns(_loaderService.Object);

        _lazyNavigationService.SetupGet(l => l.Value)
                              .Returns(_navigationService.Object);

        _mainThreadService.Setup(m => m.BeginInvokeOnMainThread(It.IsAny<Action>()))
                          .Callback<Action>(a => a.Invoke());

        ViewModel = new MainPageViewModel(
            _apiRepository.Object,
            _lazyLoaderService.Object,
            _lazyNavigationService.Object,
            _mainThreadService.Object,
            _logger.Object);
    }

    [Theory]
    [InlineData("daily", 5)]
    [InlineData("favorites", 0)]
    public async Task MainPageViewModel_TabSelectedCommandShouldLoadSpecifictabData(string tabName, int expectedItemsCount)
    {
        var postsResult = Enumerable.Repeat(new Post(), expectedItemsCount).ToArray();
        var apiResult = new ApiDataResponse()
        {
            Posts = postsResult
        };

        _apiRepository.Setup(a => a.GetApiDataAsync())
                      .ReturnsAsync(apiResult);

        await ViewModel.TabSelectedCommand.ExecuteAsync(tabName);

        ViewModel.PostCollection.Count
            .Should()
            .Be(expectedItemsCount);
    }
}