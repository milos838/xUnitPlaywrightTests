using System.Text.Json;

namespace PlaywrightTests.Utilities;

public static class TestDataLoader
{
    public static T Load<T>(string fileName)
    {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "../../../Data", fileName);
        var jsonContent = File.ReadAllText(jsonPath);
        var testData = JsonSerializer.Deserialize<T>(jsonContent);

        if (testData is null)
        {
            throw new InvalidDataException($"Test data in '{jsonPath}' deserialized to null.");
        }

        return testData;
    }
}
