using Maui.Ideas.App.Abstractions;

namespace Maui.Ideas.App.Models;

public class LazyDependency<T> : Lazy<T>, ILazyDependency<T> where T : class
{
    public LazyDependency(IServiceProvider provider)
        : base(() => provider.GetService(typeof(T)) as T)
    {
    }
}
