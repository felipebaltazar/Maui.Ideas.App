using Maui.Ideas.App.Models;

namespace Maui.Ideas.App.Abstractions;

public interface IApiRepository
{
    public Task<ApiDataResponse> GetApiDataAsync();
}
