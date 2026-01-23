using System.Text.Json.Serialization;
namespace PlaywrightTests
{
    public class TC0002_TestObject
    {
        [JsonPropertyName("URL")]
        public string? URL { get; set; }
        [JsonPropertyName("expectedTitle")]
        public string? ExpectedTitle { get; set; }
    }
}