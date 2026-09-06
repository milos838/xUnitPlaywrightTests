using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

[Collection("Stateful account tests")]
public class TC0011_Verify_Delete_from_cart_functionality: PageTest
{
    private TC0011_TestObject? testData;

    // Initializes tracing for the test
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync().ConfigureAwait(false);
        await TraceViewerComponent.StartTraceAsync(Context, this.GetType().Name);
    }

    // Disposes tracing after the test and saving the trace file
    public override async Task DisposeAsync()
    {
        await TraceViewerComponent.StopTraceAsync(Context, this.GetType().Name);
        await base.DisposeAsync().ConfigureAwait(false);
    }

    // Loads test data from the JSON file
    private void LoadTestData()
    {
        string jsonPath = Path.Combine(AppContext.BaseDirectory, "../../../Data/HomePage.json");
        string jsonContent = File.ReadAllText(jsonPath);
        testData = JsonSerializer.Deserialize<TC0011_TestObject>(jsonContent);
    }

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
        LoadTestData();
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0011: Verify Delete from Cart functionality is started!");

        await managerPage.DeleteProductFromCartAndVerifyAsync(testData!.URL!, testData!.Product!);

        Console.WriteLine("TC0011: Verify Delete from Cart functionality is completed!");
    }
}
