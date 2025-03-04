using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

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
            // Configures a Console Exporter with a period of 5 seconds.
            .AddConsoleExporter(
                (_, metricReaderOptions) =>
                {
                    metricReaderOptions
                        .PeriodicExportingMetricReaderOptions
                        .ExportIntervalMilliseconds = 5000;
                }
            )
    );

var app = builder.Build();

app.MapGet(
    "/hello-world",
    async () =>
    {
        var greetings = new Greetings.Greetings();

        return await greetings.SayHi();
    }
);

app.Run();
