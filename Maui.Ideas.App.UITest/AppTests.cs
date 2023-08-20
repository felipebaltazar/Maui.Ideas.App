using Maui.Ideas.App.UITest.Pages;
using Xappium.UITest;

namespace Maui.Ideas.App.UITest;

public class AppTests : XappiumTestBase
{
    [Fact(DisplayName = "App should launch")]
    public void AppLaunches()
    {
        new MainPage();
    }

    [Fact(DisplayName = "Should click on float button")]
    public void ClickOnFloatButton()
    {
        new MainPage()
            .ClickOnFloatButton()
            .ValidateDailyTab();
    }
}
