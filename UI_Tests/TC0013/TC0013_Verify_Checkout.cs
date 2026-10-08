using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

[Collection("Stateful account tests")]
public class TC0013_Verify_Checkout_Functionality: TracedPageTest
{
    private TC0013_TestObject? testData;

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Add the configured product to the shopping cart.
    // 4. Go to the shopping cart.
    // 5. Click Checkout.
    // 6. Verify the place order option is visible.
    //

    [Fact]
    public async Task VerifyCheckoutFunctionality()
    {
        testData = TestDataLoader.Load<TC0013_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0013: Verify Checkout functionality is started!");

        await managerPage.CheckoutAndVerifyAsync(testData!.URL!, testData!.Product!);

        Console.WriteLine("TC0013: Verify Checkout functionality is completed!");
    }
}
