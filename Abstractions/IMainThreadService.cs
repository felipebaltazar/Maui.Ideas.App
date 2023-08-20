namespace Maui.Ideas.App.Abstractions
{
    public interface IMainThreadService
    {
        bool IsMainThread { get; }

        void BeginInvokeOnMainThread(Action action);
    }
}
