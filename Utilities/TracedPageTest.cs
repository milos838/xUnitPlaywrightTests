using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

public abstract class TracedPageTest : PageTest
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync().ConfigureAwait(false);
        await TraceViewerComponent.StartTraceAsync(Context, GetType().Name);
    }

    public override async Task DisposeAsync()
    {
        await TraceViewerComponent.StopTraceAsync(Context, GetType().Name);
        await base.DisposeAsync().ConfigureAwait(false);
    }
}
