using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

[Collection("Stateful account tests")]
public class TC0012_Verify_BuyNow_functionality: TracedPageTest
{
    private TC0012_TestObject? testData;

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Add the configured product to the cart.
    // 4. Go to the shopping cart.
    // 5. Click Buy Now.
    // 6. Verify the order placement option is visible.
    //

    [Fact]
    public async Task VerifyBuyNowFunctionality()
    {
        testData = TestDataLoader.Load<TC0012_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0012: Verify Buy Now functionality is started!");

        await managerPage.BuyNowAndVerifyAsync(testData!.URL!, testData!.Product!);

        Console.WriteLine("TC0012: Verify Buy Now functionality is completed!");
    }
}
