using System.Text.Json.Serialization;
namespace PlaywrightTests;  
public class TC0011_TestObject
{
    // Ensure the property name matches the JSON key (case-sensitive)
    [JsonPropertyName("URL")]
    public string? URL { get; set; }

    [JsonPropertyName("product")]
    public string? Product { get; set; }
}
