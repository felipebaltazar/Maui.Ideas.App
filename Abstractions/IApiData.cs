using Maui.Ideas.App.Models;
using Refit;

namespace Maui.Ideas.App.Abstractions;

public interface IApiData
{
    [Get("/felipebaltazar/Maui.Ideas.App/main/db.json")]
    public Task<ApiDataResponse> GetDataAsync();
}
