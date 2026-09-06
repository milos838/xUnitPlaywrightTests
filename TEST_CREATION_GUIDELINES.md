# TEST CASE CREATION GUIDELINES

## File & Folder Structure

Each test case must follow this structure:

```
UI_Tests/
├── TC000X/                          # Replace X with test number (01-99)
│   ├── TC000X_TestObject.cs         # Data model for JSON deserialization
│   └── TC000X_Verify_*.cs           # Main test implementation
```

### Naming Convention

- **Folder:** `TC000X` (zero-padded, 4 digits: TC0001, TC0002, ... TC0015)
- **TestObject file:** `TC000X_TestObject.cs` (always this exact pattern)
- **Test file:** `TC000X_Verify_<Description>.cs` (describe what's being tested)
  - Examples: `TC0001_Verify_Login_function.cs`, `TC0010_Verify_Add_to_cart_functionality.cs`

---

## File 1: TC000X_TestObject.cs (Data Model)

**Purpose:** Serializable class to map JSON test data from `Data/HomePage.json`

### Template

```csharp
using System.Text.Json.Serialization;

namespace PlaywrightTests;

public class TC000X_TestObject
{
    [JsonPropertyName("URL")]
    public string? URL { get; set; }

    [JsonPropertyName("expectedURL")]
    public string? ExpectedURL { get; set; }

    [JsonPropertyName("expectedTitle")]
    public string? ExpectedTitle { get; set; }

    [JsonPropertyName("searchTerm")]
    public string? SearchTerm { get; set; }

    [JsonPropertyName("minPrice")]
    public string? MinPrice { get; set; }

    [JsonPropertyName("maxPrice")]
    public string? MaxPrice { get; set; }

    [JsonPropertyName("category1")]
    public string? Category1 { get; set; }

    [JsonPropertyName("category2")]
    public string? Category2 { get; set; }

    [JsonPropertyName("category3")]
    public string? Category3 { get; set; }

    [JsonPropertyName("product")]
    public string? Product { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }
}
```

### Key Points

- Add `[JsonPropertyName]` attributes matching keys in `HomePage.json`
- Use nullable strings (`string?`) for optional fields
- Properties should mirror the JSON structure exactly
- Keep namespace as `PlaywrightTests` (global namespace in this project)
- Only include properties your specific test needs; remove unused ones

---

## File 2: TC000X_Verify_<Description>.cs (Test Implementation)

**Purpose:** xUnit test class that executes the test workflow

### Template Structure

```csharp
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

/// <summary>
/// Test case: [Brief description of what's being tested]
/// Scope: [What functionality is covered]
/// </summary>
public class TC000X_Verify_<DescriptionInPascalCase> : PageTest
{
    private TC000X_TestObject? testData;

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
        testData = JsonSerializer.Deserialize<TC000X_TestObject>(jsonContent);
    }

    // Test Case Steps:
    //
    // 1. [Step 1]
    // 2. [Step 2]
    // 3. [Step 3]
    // ...
    //

    [Fact]
    public async Task Verify<DescriptionInPascalCase>()
    {
        LoadTestData();
        var managerPage = new ManagerPage(Page);

        Console.WriteLine("TC000X: Verify <description> is started!");

        // === TEST WORKFLOW ===
        // Example pattern:
        // 1. Navigate to the application
        await managerPage.NavigateToAsync(testData!.URL!);

        // 2. Perform login
        await managerPage.VerifyLoginFunctionAsync(testData!.URL!);

        // 3. Perform specific action being tested
        // [Action-specific code here]

        // 4. Verify expected outcome
        // [Assertion code here]

        Console.WriteLine("TC000X: Test Completed!");
    }
}
```

For tests that mutate shared account-backed cart or order state, add
`[Collection("Stateful account tests")]` above the test class. Keep read-only
tests ungrouped so xUnit can execute them in parallel.

### Key Points

- Class name must match filename (no spaces, PascalCase)
- Inherit from `PageTest` (from Microsoft.Playwright.Xunit)
- Always include `InitializeAsync()` and `DisposeAsync()` for trace management
- Method name decorated with `[Fact]` should match class name pattern
- Load test data in a private method, not in the test method itself
- Use `Console.WriteLine()` for test logging
- Include XML doc comments describing the test
- Add step comments before actual code

---

## Proper Code Structure & Patterns

### 1. Method Naming Convention

- Test method: `Verify<What>()` (matches `[Fact]` pattern)
- Example: `VerifyLoginFunctionality()`, `VerifyAddToCartFeature()`

### 2. Page Object Calls Pattern

```csharp
// Always use ManagerPage as the entry point
var managerPage = new ManagerPage(Page);

// Call high-level workflows from ManagerPage
await managerPage.VerifyLoginFunctionAsync(url);

// Or use HomePage/CartPage directly if needed
var homePage = new HomePage(Page);
await homePage.NavigateAsync(url);
```

### 3. Assertion Pattern

```csharp
// Use Playwright.Assertions for validations
using static Microsoft.Playwright.Assertions;

await Expect(element).ToBeVisibleAsync(new() { Timeout = 15000 });
await Expect(Page).ToHaveTitleAsync("Expected Title");
await Expect(Page).ToHaveURLAsync("https://expected-url.com");
```

### 4. Async/Await Pattern

- All I/O operations must be `async`
- Always use `await` keyword
- Use `.ConfigureAwait(false)` in Initialize/Dispose methods
- Never use `.Result` or blocking calls

### 5. Error Handling

```csharp
try
{
    // Test logic
}
catch (Exception ex)
{
    Console.WriteLine($"Error in TC000X: {ex.Message}");
    throw;
}
```

---

## Locator Strategy Guidelines

When adding new methods to `HomePage.cs` or `CartPage.cs`, follow these patterns:

### Priority Order (Most to Least Reliable)

```csharp
// 1. Role-based locators (preferred - most reliable, accessible-first)
private ILocator searchBox => _page.GetByRole(AriaRole.Textbox, new() { Name = "search" });

// 2. Test ID locators (if available in app code)
private ILocator loginButton => _page.GetByTestId("login-button");

// 3. Text-based locators (for simple elements)
private ILocator logoutLink => _page.GetByText("Sign Out");

// 4. CSS/Attribute selectors (more stable than XPath)
private ILocator cartIcon => _page.Locator(".cart-icon");

// 5. XPath locators (last resort - brittle and hard to maintain)
private ILocator complexElement => _page.Locator("//*[@id='sidebar']//input[@type='checkbox']");
```

### Method Patterns

```csharp
// Descriptive async methods
public async Task ClickLoginAsync()
{
    await loginButton.ClickAsync();
}

public async Task FillSearchAsync(string searchTerm)
{
    await searchBox.FillAsync(searchTerm);
    await searchBox.PressAsync("Enter");
}

public async Task VerifyElementVisibleAsync(ILocator element)
{
    await Expect(element).ToBeVisibleAsync();
}
```

---

## Test Data (JSON) Requirements

In `Data/HomePage.json`, add properties needed by your test:

### Example Structure

```json
{
    "URL": "https://rahulshettyacademy.com/client/#/auth/login",
    "expectedURL": "https://rahulshettyacademy.com/client/#/dashboard/dash",
    "expectedTitle": "Let's Shop",
    "searchTerm": "ADIDAS",
    "minPrice": "50000",
    "maxPrice": "60000",
    "category1": "fashion",
    "category2": "electronics",
    "category3": "household",
    "product": "ZARA COAT 3",
    "country": "Russia"
}
```

### Rules

- Use camelCase for JSON property names
- Match the `[JsonPropertyName]` in your TestObject class
- Keep test data centralized here, not hardcoded in test files
- Add new properties for new test cases
- Never commit sensitive data (credentials go in user-secrets or env vars)

---

## Complete Example: Creating TC0016

### Step 1: Create Folder

```bash
UI_Tests/TC0016/
```

### Step 2: Create TC0016_TestObject.cs

```csharp
using System.Text.Json.Serialization;

namespace PlaywrightTests;

public class TC0016_TestObject
{
    [JsonPropertyName("URL")]
    public string? URL { get; set; }

    [JsonPropertyName("product")]
    public string? Product { get; set; }

    [JsonPropertyName("wishlistItem")]
    public string? WishlistItem { get; set; }
}
```

### Step 3: Create TC0016_Verify_WishlistFeature.cs

```csharp
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text.Json;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

/// <summary>
/// Test case: Verify Wishlist Feature
/// Scope: Tests adding and removing items from wishlist
/// </summary>
public class TC0016_Verify_WishlistFeature : PageTest
{
    private TC0016_TestObject? testData;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync().ConfigureAwait(false);
        await TraceViewerComponent.StartTraceAsync(Context, this.GetType().Name);
    }

    public override async Task DisposeAsync()
    {
        await TraceViewerComponent.StopTraceAsync(Context, this.GetType().Name);
        await base.DisposeAsync().ConfigureAwait(false);
    }

    private void LoadTestData()
    {
        string jsonPath = Path.Combine(AppContext.BaseDirectory, "../../../Data/HomePage.json");
        string jsonContent = File.ReadAllText(jsonPath);
        testData = JsonSerializer.Deserialize<TC0016_TestObject>(jsonContent);
    }

    // Test Case Steps:
    //
    // 1. Navigate to application
    // 2. Login with credentials
    // 3. Search for product
    // 4. Add product to wishlist
    // 5. Verify product appears in wishlist
    //

    [Fact]
    public async Task VerifyWishlistFeature()
    {
        LoadTestData();
        var managerPage = new ManagerPage(Page);

        Console.WriteLine("TC0016: Verify Wishlist Feature is started!");

        // 1. Navigate and login
        await managerPage.VerifyLoginFunctionAsync(testData!.URL!);

        // 2. Search for product (add method to HomePage if needed)
        await managerPage.SearchProductAsync(testData!.Product!);

        // 3. Add to wishlist (add method to HomePage if needed)
        // await managerPage.AddProductToWishlistAsync(testData!.Product!);

        // 4. Verify in wishlist
        // await Expect(Page).ToContainTextAsync(testData!.Product!);

        Console.WriteLine("TC0016: Test Completed!");
    }
}
```

### Step 4: Update Data/HomePage.json

```json
{
    // ... existing properties ...
    "wishlistItem": "ADIDAS SHOES"
}
```

---

## Checklist Before Committing New Test

- [ ] Folder named `TC000X` (zero-padded, 4 digits)
- [ ] Two files created: `TC000X_TestObject.cs` and `TC000X_Verify_*.cs`
- [ ] TestObject has `[JsonPropertyName]` attributes matching JSON keys
- [ ] Test class inherits from `PageTest`
- [ ] Test class has `InitializeAsync()` and `DisposeAsync()` for trace management
- [ ] Test class has `LoadTestData()` private method
- [ ] Test method has `[Fact]` attribute and is `async Task`
- [ ] Test method name matches class name pattern: `Verify<Description>()`
- [ ] Test data added to `Data/HomePage.json`
- [ ] Uses `ManagerPage` for orchestration (entry point)
- [ ] Uses `Console.WriteLine()` for logging start and completion
- [ ] All I/O operations are `async` with `await`
- [ ] Step comments included as guide for test flow
- [ ] XML doc comment on class describing test case and scope
- [ ] No hardcoded credentials (use CredentialsHelper or user-secrets)
- [ ] Assertions use `Expect()` from Playwright.Assertions
- [ ] Locators in page objects follow priority order (role → testId → text → xpath)
- [ ] Code compiles without errors
- [ ] Test runs successfully: `dotnet test PlaywrightTests.csproj --settings Playwright.runsettings --filter FullyQualifiedName~TC000X`

---

## Viewing Test Results

### Run All Tests

```bash
dotnet test PlaywrightTests.csproj --settings Playwright.runsettings
```

### Run Specific Test

```bash
dotnet test PlaywrightTests.csproj --settings Playwright.runsettings --filter FullyQualifiedName~TC0016_Verify_WishlistFeature
```

Select a browser with `-- Playwright.BrowserName=chromium`, `firefox`, or
`webkit`. The default xUnit configuration allows up to four parallel workers;
stateful account tests remain serialized through `Utilities/StatefulTestsCollection.cs`.

### View Trace File

```bash
playwright show-trace bin/Debug/net10.0/playwright-traces/TC0016_Verify_WishlistFeature.VerifyWishlistFeature.zip
```

---

## Page Object Methods Reference

### Available Methods in HomePage

- `NavigateAsync(string url)` — Navigate to URL
- `SubmitLoginAsync()` — Login with credentials from CredentialsHelper
- `SubmitLoginAsync(string username, string password)` — Login with provided credentials
- `SearchProductAsync(string searchTerm)` — Search for product
- `SetPriceFilterAsync(string minPrice, string maxPrice)` — Filter by price
- `CheckFashionBoxAsync()`, `CheckElectronicsBoxAsync()`, `CheckHouseholdBoxAsync()` — Category filters
- `ClickCartAsync()` — Navigate to cart
- `AssertLoginFieldsVisibleAsync()` — Verify login fields are visible
- `AssertDashboardHeaderVisibleAsync()` — Verify dashboard loaded

### Available Methods in CartPage

- `DeleteProductFromCartAsync(string product)` — Remove product from cart
- `ClickBuyNowAsync()` — Click Buy Now button
- `ClickCheckoutAsync()` — Proceed to checkout
- `ClickPlaceOrderAsync()` — Place order
- `SelectCountryAsync(string country)` — Select country for shipping
- `ClickContinueShoppingAsync()` — Continue shopping

### Available Methods in ManagerPage

- `VerifyLoginFunctionAsync(string url)` — Full login workflow
- `VerifyHomePageURLAsync(string url, string expectedURL)` — Verify URL
- `NavigateToAsync(string url)` — Navigate to URL
- `NavigateAndVerifyHomeTitleAsync(string url, string expectedTitle)` — Navigate and verify title

---

## Best Practices

1. **DRY Principle** — Don't repeat locator definitions; add to page objects
2. **Meaningful Names** — Test and method names should describe what's tested
3. **Trace First** — Always initialize/dispose traces for debugging
4. **JSON Data** — Centralize test data, don't hardcode
5. **Async All the Way** — Never block threads with `.Result` or `.Wait()`
6. **Fail Fast** — Add assertions immediately after actions
7. **Comments** — Include step comments showing test flow
8. **Null Safety** — Use null-coalescing operators (`??`) and null-forgiving (`!`)
9. **Single Responsibility** — Each test should verify ONE feature
10. **Isolation** — Tests should not depend on other tests running first

---

## Troubleshooting

### Test Fails to Compile

- Verify class name matches filename (no spaces)
- Check namespace is `PlaywrightTests`
- Ensure `[JsonPropertyName]` matches JSON keys exactly
- Verify inheritance: `public class ... : PageTest`

### Test Fails at Runtime

- Check trace file: `playwright show-trace <trace-file>`
- Verify JSON properties loaded: Add `Console.WriteLine(testData!.Property)`
- Verify element locators are correct: Use browser DevTools
- Increase timeout: `new() { Timeout = 30000 }`

### JSON Deserialization Fails

- Ensure property names in JSON match `[JsonPropertyName]` exactly
- Use camelCase in JSON, PascalCase in C# with `[JsonPropertyName]`
- All properties should be `nullable` (`string?`)

---

## Quick Reference: Folder Structure

```
xUnitPlaywrightTests/
├── UI_Tests/
│   ├── TC0001/
│   │   ├── TC0001_TestObject.cs
│   │   └── TC0001_Verify_Login_function.cs
│   ├── TC0002/
│   │   ├── TC0002_TestObject.cs
│   │   └── TC0002_Verify_HomePage_Title.cs
│   ├── ...
│   └── TC0016/  ← NEW TEST
│       ├── TC0016_TestObject.cs
│       └── TC0016_Verify_WishlistFeature.cs
├── Pages/
│   ├── HomePage.cs
│   ├── CartPage.cs
│   ├── ManagerPage.cs
│   └── Components.cs (TraceViewerComponent)
├── Data/
│   └── HomePage.json  ← Add test data here
├── Utilities/
│   └── CredentialsHelper.cs
└── PlaywrightTests.csproj
```

---

**Last Updated:** 2026-06-12  
**Version:** 1.0  
**For Questions:** Refer to existing test cases (TC0001-TC0015) as reference implementations.
