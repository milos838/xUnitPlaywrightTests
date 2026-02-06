using System.Text.Json.Serialization;

namespace PlaywrightTests
{
    public class TC0005_TestObject
    {
        [JsonPropertyName("URL")]
        public string? URL { get; set; }
        
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("searchTerm")]
        public string? SearchTerm { get; set; }
        
    }
    
}