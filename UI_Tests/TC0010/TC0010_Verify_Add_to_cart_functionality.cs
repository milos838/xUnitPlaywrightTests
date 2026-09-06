using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

[Collection("Stateful account tests")]
public class TC0010_Verify_Add_to_cart_functionality: PageTest
{
    private TC0010_TestObject? testData;

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
        testData = JsonSerializer.Deserialize<TC0010_TestObject>(jsonContent);
    }

    // Test Case Steps:
    //
    // 1. Navigate to the configured URL.
    // 2. Submit login credentials.
    // 3. Add the configured product to the cart.
    // 4. Go to the shopping cart.
    // 5. Verify the product is added to the cart.
    //

    [Fact]
    public async Task VerifyAddToCartFunctionality()
    {
        LoadTestData();
        var managerPage = new Pages.ManagerPage(Page);

        Console.WriteLine("TC0010: Verify Add to Cart functionality is started!");

        await managerPage.AddProductToCartAndVerifyAsync(testData!.URL!, testData!.Product!);

        Console.WriteLine("TC0010: Verify Add to Cart functionality is completed!");
    }
}