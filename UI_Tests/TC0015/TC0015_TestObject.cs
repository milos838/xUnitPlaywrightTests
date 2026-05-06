using System.Text.Json.Serialization;
namespace PlaywrightTests;  
public class TC0015_TestObject
{
    [JsonPropertyName("URL")]
    public string? URL { get; set; }

    [JsonPropertyName("product")]
    public string? Product { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }
}