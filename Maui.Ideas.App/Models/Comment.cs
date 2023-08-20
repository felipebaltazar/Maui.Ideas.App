using Newtonsoft.Json;

namespace Maui.Ideas.App.Models;

public class Comment
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }

    [JsonProperty("postId")]
    public int PostId { get; set; }
}
