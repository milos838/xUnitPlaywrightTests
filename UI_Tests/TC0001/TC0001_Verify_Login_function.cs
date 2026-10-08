
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

public class TC0001_Verify_Login_function: TracedPageTest
{
    private TC0001_TestObject? testData;

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Verify login fields are visible.
    // 3. Submit login credentials.
    // 4. Verify the dashboard header is displayed.
    //

    [Trait("Category", "Smoke")]
    [Fact]
    public async Task VerifyLoginFunctions()
    {
        testData = TestDataLoader.Load<TC0001_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0001: Verify Login function is started!");

        await managerPage.VerifyLoginFunctionAsync(testData!.URL!);

        Console.WriteLine("TC0001: Verify Login function is completed!");
    }
}