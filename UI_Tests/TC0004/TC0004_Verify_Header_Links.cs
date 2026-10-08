
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

public class TC0004_Verify_Header_Links: TracedPageTest
{
    private TC0004_TestObject? testData;

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Verify the header links are visible.
    //

    [Fact]
    public async Task VerifyHeaderLinks()
    {
        testData = TestDataLoader.Load<TC0004_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0004: Verify Header Links is started!");

        await managerPage.VerifyHeaderLinksAsync(testData!.URL!);

        Console.WriteLine("TC0004: Verify Header Links is completed!");
    }
}