using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

[Collection("Stateful account tests")]
public class TC0010_Verify_Add_to_cart_functionality: TracedPageTest
{
    private TC0010_TestObject? testData;

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Add the configured product to the cart.
    // 4. Go to the shopping cart.
    // 5. Verify the product is added to the cart.
    //

    [Fact]
    public async Task VerifyAddToCartFunctionality()
    {
        testData = TestDataLoader.Load<TC0010_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0010: Verify Add to Cart functionality is started!");

        await managerPage.AddProductToCartAndVerifyAsync(testData!.URL!, testData!.Product!);

        Console.WriteLine("TC0010: Verify Add to Cart functionality is completed!");
    }
}