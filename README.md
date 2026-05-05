# xUnit Playwright Tests

Automated UI tests for the Rahul Shetty Academy demo web app using `Microsoft.Playwright` and `xUnit`.

## Overview

This repository contains a Playwright-based automated test suite written in C# and .NET 10. The suite uses:
- `Microsoft.Playwright.Xunit`
- `xUnit`
- `Microsoft.Extensions.Configuration.UserSecrets`
- Page Object Model for reusable UI actions

## Repository Structure

- `PlaywrightTests.csproj` — project definition and dependencies
- `UI_Tests/` — test cases grouped by TC number
- `Pages/` — page objects and shared helpers
- `Data/` — JSON test data
- `Utilities/` — helper classes like credential loading
- `CREDENTIALS_SETUP.md` — secure credentials setup guide

## Prerequisites

- .NET 10 SDK
- Playwright CLI/browser dependencies
- Windows PowerShell or a compatible command shell

## Setup

1. Restore packages:
   ```bash
   dotnet restore


## ** Credentials**

This project uses Utilities/CredentialsHelper.cs to retrieve credentials from:

User secrets
Environment variables
Supported environment variables:

TEST_USERNAME
TEST_PASSWORD
For detailed instructions, see CREDENTIALS_SETUP.md.

## **Test Data**

The suite reads data from HomePage.json. Example keys:

URL
expectedURL
expectedTitle
searchTerm
minPrice
maxPrice
category1, category2, category3
product

## **Trace Files**

The tests use Components.cs to record Playwright traces:

TraceViewerComponent.StartTraceAsync(...)
TraceViewerComponent.StopTraceAsync(...)
Trace files are saved to:

playwright-traces/

## **Test Cases**

The suite currently includes checks for:

TC0001 — Verify login function
TC0002 — Verify home page title
TC0003 — Verify home page URL
TC0004 — Verify header links
TC0005 — Verify search field
TC0006 — Verify min/max price fields
TC0007 — Verify category fields
TC0008 — Verify subcategory fields
TC0009 — Verify search checkboxes
TC0010 — Verify add to cart functionality
TC0011 — Verify delete from cart functionality
TC0012 — Verify Buy Now
TC0013 — Verify Checkout
TC0014 — Verify Continue Shopping functionality


## **Contributing**

To add a new test:

Create a new test class under UI_Tests/TC00xx/
Use existing page objects in Pages
Add or reuse JSON test data in Data
Keep naming consistent: TC00xx_Verify_<feature>.cs


**Notes**
Do not commit credentials or secret files.
Credentials should be stored securely using user secrets or CI environment variables.
The current page object model helps keep tests maintainable and reusable.
