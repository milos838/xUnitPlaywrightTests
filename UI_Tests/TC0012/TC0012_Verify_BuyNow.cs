using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using System.Reflection;
using Xunit.Sdk;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

public class TC0012_Verify_BuyNow_functionality: PageTest
{
    private TC0012_TestObject? testData;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync().ConfigureAwait(false);
        await TraceViewerComponent.StartTraceAsync(Context, this.GetType().Name);
    }

    public override async Task DisposeAsync()
    {
        await TraceViewerComponent.StopTraceAsync(Context, this.GetType().Name);
        await base.DisposeAsync().ConfigureAwait(false);
    }

    private void LoadTestData()
    {
        string jsonPath = Path.Combine(AppContext.BaseDirectory, "../../../Data/HomePage.json");
        string jsonContent = File.ReadAllText(jsonPath);
        testData = JsonSerializer.Deserialize<TC0012_TestObject>(jsonContent);
    }

    [Fact]
    public async Task VerifyBuyNowFunctionality()
    {
        LoadTestData();
        var homePage = new Pages.HomePage(Page);
        var cartPage = new CartPage(Page);

        Console.WriteLine("TC0012: Verify Buy Now functionality is started!");

        await homePage.NavigateAsync(testData!.URL!);
        await homePage.SubmitLoginAsync();
        await homePage.AddToCartAsync(testData!.Product!);
        await homePage.GoToCartAsync();
        await cartPage.AssertBuyButtonVisibleAsync();
        await cartPage.ClickBuyNowAsync();
        await cartPage.AssertPlaceOrderButtonVisibleAsync();

        Console.WriteLine("TC0012: Verify Buy Now functionality is completed!");
    }
}
