using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

[Collection("Stateful account tests")]
public class TC0011_Verify_Delete_from_cart_functionality: TracedPageTest
{
    private TC0011_TestObject? testData;

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Add the configured product to the cart.
    // 4. Go to the shopping cart.
    // 5. Delete the product from the cart.
    // 6. Verify the cart is empty.
    //

    [Fact]
    public async Task VerifyDeleteFromCartFunctionality()
    {
        testData = TestDataLoader.Load<TC0011_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0011: Verify Delete from Cart functionality is started!");

        await managerPage.DeleteProductFromCartAndVerifyAsync(testData!.URL!, testData!.Product!);

        Console.WriteLine("TC0011: Verify Delete from Cart functionality is completed!");
    }
}
