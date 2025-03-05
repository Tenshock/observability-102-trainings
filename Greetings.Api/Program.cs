using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
        // Configures `service.name` OTL property. Try to change the service name value.
        resource.AddService(serviceName: builder.Environment.ApplicationName)
    )
    // Adds OpenTelemetry Metrics Provider
    .WithMetrics(metrics =>
        metrics
            // Configures ASP .NET metrics instrumentation.
            .AddAspNetCoreInstrumentation()
            // Adds our TechnicalMeter into OTL scope.
            .AddMeter("SRP.Greetings.Api")
            // Configures a Console Exporter with a period of 5 seconds.
            .AddConsoleExporter(
                (_, metricReaderOptions) =>
                {
                    metricReaderOptions
                        .PeriodicExportingMetricReaderOptions
                        .ExportIntervalMilliseconds = 5000;
                }
            )
    )
    // Adds OpenTelemetry Tracing Provider
    .WithTracing(tracing =>
        tracing
            // Configures ASP .NET tracing instrumentation.
            .AddAspNetCoreInstrumentation()
            // Configures a Console Exporter.
            .AddConsoleExporter()
    );

var app = builder.Build();

app.MapGet(
    "/hello-world",
    async () =>
    {
        if (DateTime.Now.Second % 2 == 0)
        {
            Globals.GreetingsCounterForEvenSeconds.Add(1);
        }

        var greetings = new Greetings.Greetings();

        return await greetings.SayHi();
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
}
