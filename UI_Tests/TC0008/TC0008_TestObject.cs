using System.Text.Json.Serialization;
namespace PlaywrightTests
{
    public class TC0008_TestObject
    {
        [JsonPropertyName("URL")]
        public string? URL { get; set; }
        
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }
        [JsonPropertyName("subcategory1")]
        public string? SubCategory1 { get; set; }
        [JsonPropertyName("subcategory2")]
        public string? SubCategory2 { get; set; }
        [JsonPropertyName("subcategory3")]
        public string? SubCategory3 { get; set; }
        [JsonPropertyName("subcategory4")]
        public string? SubCategory4 { get; set; }
        [JsonPropertyName("subcategory5")]
        public string? SubCategory5 { get; set; }
    }
}