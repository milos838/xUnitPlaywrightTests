using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using System.Reflection;
using Xunit.Sdk;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

public class TC0013_Verify_Checkout_Functionality: PageTest
{
    private TC0013_TestObject? testData;

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
        testData = JsonSerializer.Deserialize<TC0013_TestObject>(jsonContent);
    }

    [Fact]
    public async Task VerifyCheckoutFunctionality()
    {
        LoadTestData();
        var homePage = new Pages.HomePage(Page);
        var cartPage = new CartPage(Page);

        Console.WriteLine("TC0013: Verify Checkout functionality is started!");

        await homePage.NavigateAsync(testData!.URL!);
        await homePage.SubmitLoginAsync();
        await homePage.AddToCartAsync(testData!.Product!);
        await homePage.GoToCartAsync();
        await cartPage.AssertCheckoutButtonVisibleAsync();
        await cartPage.ClickCheckoutAsync();
        await cartPage.AssertPlaceOrderButtonVisibleAsync();


        Console.WriteLine("TC0013: Verify Checkout functionality is completed!");
    }
}
