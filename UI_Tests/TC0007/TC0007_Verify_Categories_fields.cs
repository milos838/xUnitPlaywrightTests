using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

public class TC0007_Verify_Categories_fields: TracedPageTest
{
    private TC0007_TestObject? testData;

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Verify category checkboxes are visible.
    // 4. Select the configured categories.
    // 5. Verify the category filters are applied.
    //

    [Fact]
    public async Task VerifyCategoriesFields()
    {
        testData = TestDataLoader.Load<TC0007_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0007: Verify Categories fields is started!");

        await managerPage.VerifyCategoryFiltersAsync(testData!.URL!, testData!.Category1!, testData!.Category2!, testData!.Category3!);
        Console.WriteLine("TC0007: Verify Categories fields is completed!");
    }
}