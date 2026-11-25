using Microsoft.Playwright.Xunit;
using PlaywrightTests.Pages;

namespace PlaywrightTests;

public abstract class TracedPageTest : PageTest
{
    private bool _traceStarted;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync().ConfigureAwait(false);

        if (!string.Equals(Environment.GetEnvironmentVariable("PLAYWRIGHT_TRACE_ENABLED"),
            "true", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        await TraceViewerComponent.StartTraceAsync(Context, GetType().Name);
        _traceStarted = true;
    }

    public override async Task DisposeAsync()
    {
        try
        {
            if (_traceStarted)
            {
                await TraceViewerComponent.StopTraceAsync(Context, GetType().Name);
            }
        }
        finally
        {
            await base.DisposeAsync().ConfigureAwait(false);
        }
    }
}
