using Microsoft.Playwright;
using System.Reflection;
using System.Diagnostics;
using Xunit.Sdk;

namespace PlaywrightTests.Pages
{
    public class TraceViewerComponent
    {
        /// <summary>
        /// Gets the test method name from the call stack.
        /// </summary>
        private static string GetTestMethodName()
        {
            var stackTrace = new StackTrace(2);
            if (stackTrace.FrameCount > 0)
            {
                var frame = stackTrace.GetFrame(0);
                return frame?.GetMethod()?.Name ?? "UnknownTest";
            }
            return "UnknownTest";
        }

        /// <summary>
        /// Starts tracing for Playwright with screenshots, snapshots, and sources.
        /// </summary>
        public static async Task StartTraceAsync(IBrowserContext context, string className)
        {
            var testName = GetTestMethodName();
            await context.Tracing.StartAsync(new()
            {
                Title = $"{className}.{testName}",
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        /// <summary>
        /// Starts tracing for Playwright with screenshots, snapshots, and sources.
        /// </summary>
        public static async Task StartTraceAsync(IBrowserContext context, string className, string testName)
        {
            await context.Tracing.StartAsync(new()
            {
                Title = $"{className}.{testName}",
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
            var testName = GetTestMethodName();
            var traceDirectory = Path.Combine(Environment.CurrentDirectory, "playwright-traces");
            Directory.CreateDirectory(traceDirectory);
            await context.Tracing.StopAsync(new()
            {
                Path = Path.Combine(traceDirectory, $"{className}.{testName}.zip")
            });
        }

        /// <summary>
        /// Stops tracing and saves the trace file to the playwright-traces directory.
        /// </summary>
        public static async Task StopTraceAsync(IBrowserContext context, string className, string testName)
        {
            var traceDirectory = Path.Combine(Environment.CurrentDirectory, "playwright-traces");
            Directory.CreateDirectory(traceDirectory);
            await context.Tracing.StopAsync(new()
            {
                Path = Path.Combine(traceDirectory, $"{className}.{testName}.zip")
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
