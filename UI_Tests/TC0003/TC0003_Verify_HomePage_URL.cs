
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using System.Reflection;
using Xunit.Sdk;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

public class TC0003_Verify_HomePage_URL: PageTest
{
    private TC0003_TestObject? testData;

    // Initializes tracing for the test
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync().ConfigureAwait(false);
        await TraceViewerComponent.StartTraceAsync(Context, this.GetType().Name);
    }
    // Disposes tracing after the test and saving the trace file
    public override async Task DisposeAsync()
    {
        await TraceViewerComponent.StopTraceAsync(Context, this.GetType().Name);
        await base.DisposeAsync().ConfigureAwait(false);
    }

    // Loads test data from the JSON file
    private void LoadTestData()
    {
        string jsonPath = Path.Combine(AppContext.BaseDirectory, "../../../Data/HomePage.json");
        string jsonContent = File.ReadAllText(jsonPath);
        testData = JsonSerializer.Deserialize<TC0003_TestObject>(jsonContent);
    }

    [Fact]
    public async Task VerifyHomePageURL()
    {
        LoadTestData();
        var homePage = new Pages.HomePage(Page);

        Console.WriteLine("TC0003: Verify HomePage URL is started!");

        await homePage.NavigateAsync(testData!.URL!);
        await homePage.SubmitLoginAsync();
        await homePage.AssertURLAsync(testData!.ExpectedURL!);

        Console.WriteLine("TC0003: Verify HomePage URL is completed!");
    }
}