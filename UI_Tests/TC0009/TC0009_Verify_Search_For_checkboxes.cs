using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

public class TC0009_Verify_Search_For_checkboxes: TracedPageTest
{
    private TC0009_TestObject? testData;

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Verify "Search For Men" and "Search For Women" checkboxes are visible.
    // 4. Select both checkboxes.
    // 5. Verify both checkbox selections are applied.
    //

    [Fact]
    public async Task VerifySearchForCheckboxes()
    {
        testData = TestDataLoader.Load<TC0009_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0009: Verify Search For checkboxes is started!");

        await managerPage.VerifySearchForCheckboxesAsync(testData!.URL!);

        Console.WriteLine("TC0009: Verify Search For checkboxes is completed!");
    }
}