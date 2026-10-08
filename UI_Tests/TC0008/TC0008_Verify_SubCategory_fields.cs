using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;
using PlaywrightTests.Utilities;

namespace PlaywrightTests;

public class TC0008_Verify_SubCategory_fields: TracedPageTest
{
    private TC0008_TestObject? testData;

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Verify subcategory checkboxes are visible.
    // 4. Select the configured subcategories.
    // 5. Verify the subcategory selections are applied.
    //

    [Fact]
    public async Task VerifySubCategoriesFields()
    {
        testData = TestDataLoader.Load<TC0008_TestObject>("HomePage.json");
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0008: Verify Sub Categories fields is started!");

        await managerPage.VerifySubCategoryFiltersAsync(testData!.URL!);
        Console.WriteLine("TC0008: Verify Sub Categories fields is completed!");
    }
}