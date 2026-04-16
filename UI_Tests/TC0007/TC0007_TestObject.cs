using System.Text.Json.Serialization;
namespace PlaywrightTests
{
    public class TC0007_TestObject
    {
        [JsonPropertyName("URL")]
        public string? URL { get; set; }
        
        [JsonPropertyName("category1")]
        public string? Category1 { get; set; }
        [JsonPropertyName("category2")]
        public string? Category2 { get; set; }
        [JsonPropertyName("category3")]
        public string? Category3 { get; set; }
    }
}