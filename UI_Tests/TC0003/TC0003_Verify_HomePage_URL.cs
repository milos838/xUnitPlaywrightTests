
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

public class TC0003_Verify_HomePage_URL: TracedPageTest
{
    private TC0003_TestObject? testData;

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Verify the current URL matches the expected home page URL.
    //

    [Fact]
    public async Task VerifyHomePageURL()
    {
        testData = TestDataLoader.Load<TC0003_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0003: Verify HomePage URL is started!");

        await managerPage.VerifyHomePageURLAsync(testData!.URL!, testData!.ExpectedURL!);

        Console.WriteLine("TC0003: Verify HomePage URL is completed!");
    }
}