using System.Text.Json.Serialization;

namespace PlaywrightTests
{
    public class TC0001_TestObject
    {
        [JsonPropertyName("URL")]
        public string? URL { get; set; }
        [JsonPropertyName("expectedURL")]
        public string? ExpectedURL { get; set; }
    }
}