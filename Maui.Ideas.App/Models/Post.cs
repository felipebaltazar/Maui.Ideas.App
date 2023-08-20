using Newtonsoft.Json;

namespace Maui.Ideas.App.Models;

public class Post
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("user")]
    public User User { get; set; }
}
