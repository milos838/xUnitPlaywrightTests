using System.Text.Json.Serialization;

namespace PlaywrightTests
{
    public class TC0003_TestObject
    {
        [JsonPropertyName("URL")]
        public string? URL { get; set; }
        [JsonPropertyName("expectedURL")]
        public string? ExpectedURL { get; set; }
    }
}