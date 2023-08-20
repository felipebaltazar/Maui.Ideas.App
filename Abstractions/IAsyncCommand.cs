using System.Windows.Input;

namespace Maui.Ideas.App.Abstractions;

public interface IAsyncCommand<T> : ICommand
{
    Task ExecuteAsync(T parameter);

    void RaiseCanExecuteChanged();
}

public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync();

    void RaiseCanExecuteChanged();
}
