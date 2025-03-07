using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder
    .Logging.ClearProviders()
    .Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
        resource.AddService(serviceName: builder.Environment.ApplicationName)
    )
    .WithLogging(logging => logging.AddConsoleExporter())
    .WithMetrics(metrics =>
        metrics
            .AddAspNetCoreInstrumentation()
            .AddMeter("SRP.Greetings.Api")
            .AddConsoleExporter(
                (_, metricReaderOptions) =>
                {
                    metricReaderOptions
                        .PeriodicExportingMetricReaderOptions
                        .ExportIntervalMilliseconds = 1000;
                }
            )
    )
    .WithTracing(tracing =>
        tracing.AddAspNetCoreInstrumentation().AddSource("SRP.Greetings.Api").AddConsoleExporter()
    );

builder.Services.AddScoped<Greetings.Greetings>();

var app = builder.Build();

app.MapGet(
    "/hello-world",
    async (Greetings.Greetings greetings) =>
    {
        if (DateTime.Now.Second % 2 == 0)
        {
            Globals.GreetingsCounterForEvenSeconds.Add(1);
        }

        using (var activity = Globals.GreetingsApiActivitySource.StartActivity("SayHi"))
        {
            return await greetings.SayHi();
        }
    }
);

app.Run();

/// <summary>
/// Static class containing global references towards metrics entities.
/// </summary>
/// <remarks>
/// Storing surch references statically is for educational purpose.
/// Other logics like DI may be also used.
/// </remarks>
public static class Globals
{
    /// <summary>
    /// A Meter gathering technical-oriented metrics.
    /// </summary>
    /// <see href="https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/metrics" />
    private static readonly Meter TechnicalMeter = new("SRP.Greetings.Api", "0.1.0");

    /// <summary>
    /// A counter whose purpose is to count all Greetings calls that happen at even seconds (because, why not?).
    /// </summary>
    public static readonly Counter<int> GreetingsCounterForEvenSeconds =
        TechnicalMeter.CreateCounter<int>("GreetingsCounterForEvenSeconds");

    /// <summary>
    /// An ActivitySource for the application scope.
    /// </summary>
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activitysource?view=net-9.0" />
    /// <see href="https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace" />
    public static readonly ActivitySource GreetingsApiActivitySource = new("SRP.Greetings.Api");
}
