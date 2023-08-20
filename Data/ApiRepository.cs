using Maui.Ideas.App.Abstractions;
using Maui.Ideas.App.Models;

namespace Maui.Ideas.App.Data
{
    public sealed class ApiRepository : ObjectWithPolicy, IApiRepository
    {
        private readonly IApiData _api;

        public ApiRepository(IApiData apiData) 
        {
            _api = apiData;
        }

        public Task<ApiDataResponse> GetApiDataAsync() =>
            RequestWithPolicy(_api.GetDataAsync);
    }
}
