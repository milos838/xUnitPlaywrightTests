using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using System.Reflection;
using Xunit.Sdk;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

public class TC0006_Verify_Min_and_Max_fields: PageTest
{
    private TC0006_TestObject? testData;

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
        testData = JsonSerializer.Deserialize<TC0006_TestObject>(jsonContent);
    }

    [Fact]
    public async Task VerifyMinAndMaxFields()
    {
        LoadTestData();
        var homePage = new Pages.HomePage(Page);

        Console.WriteLine("TC0006: Verify Min and Max fields is started!");

        await homePage.NavigateAsync(testData!.URL!);
        await homePage.SubmitLoginAsync();
        await homePage.AssertPriceFilterFieldsVisibleAsync();
        await homePage.SetPriceFilterAsync(testData!.MinPrice!, testData!.MaxPrice!);
        await homePage.AssertPriceSearchedProductVisibleAsync();
        Console.WriteLine("TC0006: Verify Min and Max fields is completed!");
    }
}