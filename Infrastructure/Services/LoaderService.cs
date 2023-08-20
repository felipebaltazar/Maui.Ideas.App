using AsyncAwaitBestPractices;
using Maui.Ideas.App.Abstractions;
using Maui.Ideas.App.Presentation.Views.Controls;
using Mopups.Interfaces;

namespace Maui.Ideas.App.Infrastructure.Services
{
    public class LoaderService : ILoaderService
    {
        private Lazy<LoaderPopup> _loaderPopUp = new Lazy<LoaderPopup>(() => new LoaderPopup());

        private IPopupNavigation _popupNavigation;

        public LoaderService(IPopupNavigation popupNavigation)
        {
            _popupNavigation = popupNavigation;
        }

        public void HideLoading() =>
            _popupNavigation.PopAsync().SafeFireAndForget();

        public void ShowLoading()
        {
            if (_popupNavigation.PopupStack.Any(p=> p.GetType() == typeof(LoaderPopup)))
                return;

            _popupNavigation.PushAsync(_loaderPopUp.Value).SafeFireAndForget();
        }
    }
}
