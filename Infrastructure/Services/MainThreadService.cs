using Maui.Ideas.App.Abstractions;

namespace Maui.Ideas.App.Infrastructure.Services
{
    public sealed class MainThreadService : IMainThreadService
    {
        public bool IsMainThread => MainThread.IsMainThread;

        public void BeginInvokeOnMainThread(Action action) => MainThread.BeginInvokeOnMainThread(action);
    }
}
