using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

public class TC0014_Verify_ContinueShopping_Functionality: PageTest
{
    private TC0014_TestObject? testData;

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
        testData = JsonSerializer.Deserialize<TC0014_TestObject>(jsonContent);
    }

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Add the configured product to the shopping cart.
    // 4. Go to the shopping cart.
    // 5. Click Continue Shopping.
    // 6. Verify the dashboard is visible again.
    //

    [Fact]
    public async Task VerifyContinueShoppingFunctionality()
    {
        LoadTestData();
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0014: Verify ContinueShopping functionality is started!");

        await managerPage.ContinueShoppingAndVerifyAsync(testData!.URL!, testData!.Product!);

        Console.WriteLine("TC0014: Verify ContinueShopping functionality is completed!");
    }
}
