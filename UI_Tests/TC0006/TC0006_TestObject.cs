using System.Text.Json.Serialization;

namespace PlaywrightTests
{
    public class TC0006_TestObject
    {
        [JsonPropertyName("URL")]
        public string? URL { get; set; }
        
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }
        
        [JsonPropertyName("minPrice")]
        public string? MinPrice { get; set; }
        
        [JsonPropertyName("maxPrice")]
        public string? MaxPrice { get; set; }
    }
}