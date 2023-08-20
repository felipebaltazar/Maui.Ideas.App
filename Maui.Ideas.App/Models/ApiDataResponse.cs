using Newtonsoft.Json;

namespace Maui.Ideas.App.Models;

public class ApiDataResponse
{
    [JsonProperty("posts")]
    public Post[] Posts { get; set; }

    [JsonProperty("comments")]
    public Comment[] Comments { get; set; }

    [JsonProperty("profile")]
    public Profile Profile { get; set; }
}
