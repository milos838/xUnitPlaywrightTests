using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using System.Reflection;
using Xunit.Sdk;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

public class TC0007_Verify_Categories_fields: PageTest
{
    private TC0007_TestObject? testData;

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
        testData = JsonSerializer.Deserialize<TC0007_TestObject>(jsonContent);
    }

    [Fact]
    public async Task VerifyCategoriesFields()
    {
        LoadTestData();
        var homePage = new Pages.HomePage(Page);

        Console.WriteLine("TC0007: Verify Categories fields is started!");

        await homePage.NavigateAsync(testData!.URL!);
        await homePage.SubmitLoginAsync();
        await homePage.AssertCategoryCheckBoxesVisibleAsync();
        await homePage.CheckFashionBoxAsync();
        await homePage.CheckElectronicsBoxAsync();
        await homePage.CheckHouseholdBoxAsync();    
        await homePage.AssertCategoryCheckBoxCheckedAsync(testData!.Category1!);
        await homePage.AssertCategoryCheckBoxCheckedAsync(testData!.Category2!);
        await homePage.AssertCategoryCheckBoxCheckedAsync(testData!.Category3!);
        Console.WriteLine("TC0007: Verify Categories fields is completed!");
    }
}