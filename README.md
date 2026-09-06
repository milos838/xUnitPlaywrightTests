# xUnit Playwright Tests

## Description

A Playwright-based automated UI test suite for the Rahul Shetty Academy demo e-commerce web application. Built with C#, .NET 10, and xUnit, this project uses a Page Object Model architecture to centralize page actions, secure credential handling, and trace-based debugging.

## Table of Contents

- [Description](#description)
- [Repository Structure](#repository-structure)
- [Installation](#installation)
- [Usage](#usage)
- [Features](#features)
- [Configuration](#configuration)
- [CI/CD](#cicd)
- [Contributing](#contributing)
- [Notes](#notes)

## Repository Structure

- `PlaywrightTests.csproj` — project definition and dependency list
- `UI_Tests/` — test classes organized by TC number
- `Pages/` — page object implementations and tracing helpers
- `Data/` — JSON-driven test input data
- `Utilities/` — helper utilities such as credentials management and xUnit collection definitions
- `Playwright.runsettings` — default browser and Playwright launch settings
- `xunit.runner.json` — xUnit parallel execution settings
- `.github/workflows/playwright-tests.yml` — GitHub Actions test pipeline
- `CREDENTIALS_SETUP.md` — secure credential setup guide

## Installation

### Prerequisites

- .NET 10 SDK
- Playwright browser dependencies
- Windows PowerShell or compatible shell

### Setup Steps

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd xUnitPlaywrightTests
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Build and install Playwright browsers**:
   ```bash
   dotnet build --configuration Debug
   pwsh bin/Debug/net10.0/playwright.ps1 install
   ```

   For a Release build, use `bin/Release/net10.0/playwright.ps1` instead.

4. **Configure credentials** (see [Configuration](#configuration)).

## Usage

### Running Tests

Run all tests with the checked-in settings:
```bash
dotnet test PlaywrightTests.csproj --settings Playwright.runsettings
```

Run without the checked-in settings:
```bash
dotnet test PlaywrightTests.csproj
```

The suite runs independent xUnit collections concurrently, capped at four worker
threads by `xunit.runner.json`. TC0010 through TC0015 use the shared `Stateful
account tests` collection in `Utilities/StatefulTestsCollection.cs`; those tests
remain serialized because they use the same account-backed cart and order state.
Each stateful workflow also clears the existing cart before starting.

Run against a specific browser:
```powershell
dotnet test PlaywrightTests.csproj --settings Playwright.runsettings -- Playwright.BrowserName=chromium
dotnet test PlaywrightTests.csproj --settings Playwright.runsettings -- Playwright.BrowserName=firefox
dotnet test PlaywrightTests.csproj --settings Playwright.runsettings -- Playwright.BrowserName=webkit
```

Use a headed browser locally:
```powershell
$env:HEADED = "1"
dotnet test PlaywrightTests.csproj --settings Playwright.runsettings -- Playwright.BrowserName=chromium
Remove-Item Env:HEADED
```

Run one test:
```powershell
dotnet test PlaywrightTests.csproj --settings Playwright.runsettings --filter FullyQualifiedName~TC0014_Verify_ContinueShopping_Functionality
```

For CI, run Chromium, Firefox, and WebKit as separate matrix jobs. Do not mix
browsers in one test process.

### Viewing Traces

Playwright traces are recorded for each test and saved to `playwright-traces/`.
Use the Playwright trace viewer to inspect a trace:
```bash
playwright show-trace playwright-traces/TC0014_Verify_ContinueShopping_Functionality.VerifyContinueShoppingFunctionality.zip
```

### Debugging Failed Tests

- Review trace files in `playwright-traces/`
- Consult the page object methods in `Pages/HomePage.cs` and `Pages/CartPage.cs`
- Check the JSON data values in `Data/HomePage.json`

## Features

- **15 automated test cases** covering login, search, filtering, cart behavior, checkout, and continue shopping flows
- **Page Object Model** for reusable page actions in `Pages/`
- **Secure credential management** using user secrets or environment variables
- **Trace recording** with screenshots, snapshots, and sources
- **Centralized test input** in `Data/HomePage.json`
- **Stable cart workflows** with deterministic product rendering, country selection, and cart-row deletion
- **GitHub Actions integration** for CI/CD validation

## Configuration

### Credentials

Credentials are loaded from `Utilities/CredentialsHelper.cs`. Supported sources:

1. **User Secrets** (recommended for local development):
   ```bash
   dotnet user-secrets set "Credentials:Username" "<username>"
   dotnet user-secrets set "Credentials:Password" "<password>"
   ```

2. **Environment Variables** (for CI/CD):
   ```powershell
   $env:TEST_USERNAME = "<username>"
   $env:TEST_PASSWORD = "<password>"
   ```

For detailed instructions, see `CREDENTIALS_SETUP.md`.

> On GitHub Actions, set repository secrets named `TEST_USERNAME` and `TEST_PASSWORD` so CI can run login tests without storing credentials in source control.

### Test Data

Data values are defined in `Data/HomePage.json`.
Update these fields to change the test inputs:
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
  "product": "ZARA COAT 3"
}
```

## CI/CD

The repository includes a GitHub Actions workflow at `.github/workflows/playwright-tests.yml`.

The workflow:
- checks out the code
- sets up .NET 10
- restores dependencies
- builds the project
- installs Playwright browsers using `pwsh bin/Release/net10.0/playwright.ps1 install`
- runs tests and generates a TRX report
- uploads test results as an artifact
- publishes results using the EnricoMi action

The local `Playwright.runsettings` file defaults to headless Chromium. Browser
selection can be overridden with `Playwright.BrowserName=chromium`, `firefox`,
or `webkit`.

## Contributing

To add a new test:
1. Create a new folder under `UI_Tests/TC00xx/`.
2. Add a `_TestObject.cs` file for JSON data deserialization.
3. Add a `_Verify_<feature>.cs` file for test logic.
4. Use existing page objects from `Pages/`.
5. Update `Data/HomePage.json` with new test values.
6. Keep read-only tests outside the `Stateful account tests` collection so they can run in parallel.
7. Add account-mutating tests to `Stateful account tests` and reset backend state before each workflow.
8. Follow naming conventions: `TC00xx_Verify_<Feature>.cs`.

## Notes

- Do not commit credentials, secrets, or `.env` files.
- Use secure storage for credentials and environment configuration.
- `Pages/HomePage.cs` and `Pages/CartPage.cs` contain the main reusable UI actions.
- `Pages/ManagerPage.cs` coordinates workflows and resets the cart before stateful flows.
- `Utilities/StatefulTestsCollection.cs` serializes tests that share account-backed state.
- Trace artifacts are stored in `playwright-traces/` and can be opened with Playwright Inspector.
