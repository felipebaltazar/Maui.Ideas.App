using Newtonsoft.Json;

namespace Maui.Ideas.App.Models;

public class Profile
{
    [JsonProperty("name")]
    public string Name { get; set; }
}
