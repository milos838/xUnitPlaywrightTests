using Microsoft.Playwright;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace xUnitPlaywrightTests.Tests
{
    public class TC001_LoginValidation : IAsyncLifetime
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IBrowserContext? _context;
        private IPage? _page;

        public async Task InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();

            await _context.Tracing.StartAsync(new()
            {
                Title = $"{WithTestNameAttribute.CurrentClassName}.{WithTestNameAttribute.CurrentTestName}",
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        public async Task DisposeAsync()
        {
            if (_context != null)
            {
                await _context.Tracing.StopAsync(new()
                {
                    Path = Path.Combine(
                        Environment.CurrentDirectory,
                        "playwright-traces",
                        $"{WithTestNameAttribute.CurrentClassName}.{WithTestNameAttribute.CurrentTestName}.zip"
                    )
                });
                await _context.CloseAsync();
            }
            if (_browser != null)
                await _browser.CloseAsync();
            _playwright?.Dispose();
        }

        [Fact]
        // TC001 - Verify Login functionality
        public async Task HasTitle()
        {
            Assert.NotNull(_page);
            await _page.GotoAsync("https://playwright.dev");
            var title = await _page.TitleAsync();
            Assert.Matches(new Regex("Playwright"), title);
        }
    }
}

public class WithTestNameAttribute : BeforeAfterTestAttribute
{
    public static string CurrentTestName = string.Empty;
    public static string CurrentClassName = string.Empty;

    public override void Before(MethodInfo methodInfo)
    {
        CurrentTestName = methodInfo.Name;
        CurrentClassName = methodInfo.DeclaringType!.Name;
    }

    public override void After(MethodInfo methodInfo)
    {
    }
}