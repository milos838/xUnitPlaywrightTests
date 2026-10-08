using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

public class TC0006_Verify_Min_and_Max_fields: TracedPageTest
{
    private TC0006_TestObject? testData;

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Verify the price filter fields are visible.
    // 4. Set the minimum and maximum price filters.
    // 5. Verify the filtered product results are visible.
    //

    [Fact]
    public async Task VerifyMinAndMaxFields()
    {
        testData = TestDataLoader.Load<TC0006_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0006: Verify Min and Max fields is started!");

        await managerPage.VerifyPriceFilterAsync(testData!.URL!, testData!.MinPrice!, testData!.MaxPrice!);
        Console.WriteLine("TC0006: Verify Min and Max fields is completed!");
    }
}