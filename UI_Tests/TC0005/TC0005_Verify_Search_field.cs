using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

public class TC0005_Verify_Search_Field: TracedPageTest
{
    private TC0005_TestObject? testData;

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Verify the search field is visible.
    // 4. Search for the configured term.
    // 5. Verify the searched product is visible.
    //

    [Fact]
    public async Task VerifySearchField()
    {
        testData = TestDataLoader.Load<TC0005_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0005: Verify Search Field is started!");

        await managerPage.VerifySearchFieldAsync(testData!.URL!, testData!.SearchTerm!);

        Console.WriteLine("TC0005: Verify Search Field is completed!");
    }
}