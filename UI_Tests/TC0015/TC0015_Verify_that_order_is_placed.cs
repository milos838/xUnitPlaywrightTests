using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

public class TC0015_Verify_that_order_is_placed: PageTest
{
    private TC0015_TestObject? testData;

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
        testData = JsonSerializer.Deserialize<TC0015_TestObject>(jsonContent);
    }

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Add the configured product to the shopping cart.
    // 4. Go to the shopping cart.
    // 5. Click Checkout.
    // 6. Fill in the country field with the configured country.
    // 7. Click Place Order.
    // 8. Verify that the order is placed successfully.
    //

    [Fact]
    public async Task VerifyThatOrderIsPlaced()
    {
        LoadTestData();
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0015: Verify that order is placed is started!");

        await managerPage.PlaceOrderAndVerifyAsync(testData!.URL!, testData!.Product!, testData!.Country!);

        Console.WriteLine("TC0015: Verify that order is placed is completed!");
    }
}