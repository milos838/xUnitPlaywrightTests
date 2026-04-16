using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using System.Reflection;
using Xunit.Sdk;
using PlaywrightTests.Pages;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests;

public class TC0011_Verify_Delete_from_cart_functionality: PageTest
{
    private TC0011_TestObject? testData;

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
        testData = JsonSerializer.Deserialize<TC0011_TestObject>(jsonContent);
    }

    [Fact]
    public async Task VerifyDeleteFromCartFunctionality()
    {
        LoadTestData();
        var homePage = new Pages.HomePage(Page);
        var cartPage = new CartPage(Page);

        Console.WriteLine("TC0011: Verify Delete from Cart functionality is started!");

        await homePage.NavigateAsync(testData!.URL!);
        await homePage.SubmitLoginAsync(testData!.Username!, testData!.Password!);
        await homePage.AddToCartAsync(testData!.Product!);
        await homePage.GoToCartAsync();
        await cartPage.AssertBuyButtonVisibleAsync();
        await cartPage.DeleteProductFromCartAsync(testData!.Product!);
        await cartPage.AssertNoCartItemsMessageVisibleAsync();
        

        Console.WriteLine("TC0011: Verify Delete from Cart functionality is completed!");
    }
}
