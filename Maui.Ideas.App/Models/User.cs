using Newtonsoft.Json;

namespace Maui.Ideas.App.Models;

public class User
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("isVerfied")]
    public bool IsVerfied { get; set; }

    [JsonProperty("avatar")]
    public string Avatar { get; set; }
}
