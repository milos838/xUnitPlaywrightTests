using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

[Collection("Stateful account tests")]
public class TC0014_Verify_ContinueShopping_Functionality: TracedPageTest
{
    private TC0014_TestObject? testData;

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
        testData = TestDataLoader.Load<TC0014_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0014: Verify ContinueShopping functionality is started!");

        await managerPage.ContinueShoppingAndVerifyAsync(testData!.URL!, testData!.Product!);

        Console.WriteLine("TC0014: Verify ContinueShopping functionality is completed!");
    }
}
