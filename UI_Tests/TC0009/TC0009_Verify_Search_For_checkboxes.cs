using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using System.Reflection;
using Xunit.Sdk;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

public class TC0009_Verify_Search_For_checkboxes: PageTest
{
    private TC0009_TestObject? testData;

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
        testData = JsonSerializer.Deserialize<TC0009_TestObject>(jsonContent);
    }

    [Fact]
    public async Task VerifySearchForCheckboxes()
    {
        LoadTestData();
        var homePage = new Pages.HomePage(Page);

        Console.WriteLine("TC0009: Verify Search For checkboxes is started!");

        await homePage.NavigateAsync(testData!.URL!);
        await homePage.SubmitLoginAsync(testData!.Username!, testData!.Password!);
        await homePage.AssertSearchForMenBoxVisibleAsync();
        await homePage.AssertSearchForWomenBoxVisibleAsync();
        await homePage.CheckSearchForMenBoxAsync();
        await homePage.AssertSearchForMenBoxCheckedAsync();
        await homePage.CheckSearchForWomenBoxAsync();
        await homePage.AssertSearchForWomenBoxCheckedAsync();

        Console.WriteLine("TC0009: Verify Search For checkboxes is completed!");
    }
}