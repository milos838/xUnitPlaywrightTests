
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

public class TC0002_Verify_HomePage_Title: TracedPageTest
{
    private TC0002_TestObject? testData;

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Verify the home page title matches the expected title.
    //
    [Trait("Category", "Smoke")]
    [Fact]
    public async Task VerifyHomePageTitle()
    {
        testData = TestDataLoader.Load<TC0002_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0002: Verify HomePage Title is started!");

        await managerPage.NavigateAndVerifyHomeTitleAsync(testData!.URL!, testData!.ExpectedTitle!);

        Console.WriteLine("TC0002: Verify HomePage Title is completed!");
    }
}