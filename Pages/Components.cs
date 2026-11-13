using Microsoft.Playwright;
using System.Reflection;
using Xunit.Sdk;

namespace PlaywrightTests.Pages
{
    public class TraceViewerComponent
    {
        /// <summary>
        /// Starts tracing for Playwright with screenshots, snapshots, and sources.
        /// </summary>
        public static async Task StartTraceAsync(IBrowserContext context, string className)
        {
            await StartTraceAsync(context, className, className);
        }

        /// <summary>
        /// Starts tracing for Playwright with screenshots, snapshots, and sources.
        /// </summary>
        public static async Task StartTraceAsync(IBrowserContext context, string className, string testName)
        {
            var traceIdentifier = className == testName ? className : $"{className}.{testName}";
            await context.Tracing.StartAsync(new()
            {
                Title = traceIdentifier,
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        /// <summary>
        /// Stops tracing and saves the trace file to the playwright-traces directory.
        /// </summary>
        public static async Task StopTraceAsync(IBrowserContext context, string className)
        {
            await StopTraceAsync(context, className, className);
        }

        /// <summary>
        /// Stops tracing and saves the trace file to the playwright-traces directory.
        /// </summary>
        public static async Task StopTraceAsync(IBrowserContext context, string className, string testName)
        {
            var traceDirectory = Path.Combine(Environment.CurrentDirectory, "playwright-traces");
            Directory.CreateDirectory(traceDirectory);
            var traceIdentifier = className == testName ? className : $"{className}.{testName}";
            await context.Tracing.StopAsync(new()
            {
                Path = Path.Combine(traceDirectory, $"{traceIdentifier}.zip")
            });
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
}
