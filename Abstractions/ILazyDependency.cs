namespace Maui.Ideas.App.Abstractions;

public interface ILazyDependency<T>
{
    T Value { get; }
}
