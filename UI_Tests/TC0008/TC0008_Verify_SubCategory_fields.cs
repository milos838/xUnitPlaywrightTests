using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using System.Reflection;
using Xunit.Sdk;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

public class TC0008_Verify_SubCategory_fields: PageTest
{
    private TC0008_TestObject? testData;

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
        testData = JsonSerializer.Deserialize<TC0008_TestObject>(jsonContent);
    }

    [Fact]
    public async Task VerifySubCategoriesFields()
    {
        LoadTestData();
        var homePage = new Pages.HomePage(Page);

        Console.WriteLine("TC0008: Verify Sub Categories fields is started!");

        await homePage.NavigateAsync(testData!.URL!);
        await homePage.SubmitLoginAsync();
        await homePage.AssertSubCategoryCheckBoxesVisibleAsync();
        await homePage.CheckTShirtsBoxAsync();  
        await homePage.AssertSubCategoryTShirtsCheckBoxCheckedAsync();
        await homePage.CheckShirtsBoxAsync();
        await homePage.AssertSubCategoryShirtsCheckBoxCheckedAsync(); 
        await homePage.CheckShoesBoxAsync();
        await homePage.AssertSubCategoryShoesCheckBoxCheckedAsync();
        await homePage.CheckMobilesBoxAsync();
        await homePage.AssertSubCategoryMobilesCheckBoxCheckedAsync();
        await homePage.CheckLaptopsBoxAsync();
        await homePage.AssertSubCategoryLaptopsCheckBoxCheckedAsync();
        Console.WriteLine("TC0008: Verify Sub Categories fields is completed!");
    }
}