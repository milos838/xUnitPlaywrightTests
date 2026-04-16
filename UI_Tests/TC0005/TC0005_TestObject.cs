using System.Text.Json.Serialization;

namespace PlaywrightTests
{
    public class TC0005_TestObject
    {
        [JsonPropertyName("URL")]
        public string? URL { get; set; }
        
        [JsonPropertyName("searchTerm")]
        public string? SearchTerm { get; set; }
    }
    
}