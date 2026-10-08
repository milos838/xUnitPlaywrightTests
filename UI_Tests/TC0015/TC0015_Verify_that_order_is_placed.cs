using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

[Collection("Stateful account tests")]
public class TC0015_Verify_that_order_is_placed: TracedPageTest
{
    private TC0015_TestObject? testData;

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

    [Trait("Category", "Smoke")]
    [Fact]
    public async Task VerifyThatOrderIsPlaced()
    {
        testData = TestDataLoader.Load<TC0015_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0015: Verify that order is placed is started!");

        await managerPage.PlaceOrderAndVerifyAsync(testData!.URL!, testData!.Product!, testData!.Country!);

        Console.WriteLine("TC0015: Verify that order is placed is completed!");
    }
}