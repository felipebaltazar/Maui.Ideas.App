using Xappium.UITest;
using Xappium.UITest.Pages;

namespace Maui.Ideas.App.UITest.Pages
{
    public class MainPage : BasePage
    {
        protected override string Trait => "TitleMessage";

        public MainPage ValidateDailyTab()
        {
            Engine.AssertTextInElementByAutomationId("TitleMessage", "Ideas");
            Engine.Screenshot("Has title message");

            return this;
        }

        public MainPage ClickOnFloatButton() 
        {
            Engine.Tap("FloatButton");
            Engine.Screenshot("FloatButton animation");
            return this;
        }
    }
}
