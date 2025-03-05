# Observability-102 First Hands-on

## Getting started

Go to the first commit and follow step by step the correction following commits.

> Hints: Hands-on instructions are listed below, the current commit instruction is highlighted. Also, commits messages references the instructions numbering with a command to go to next commit.

## Instructions

1. create a new `.NET` Solution.
2. Add a .NET Class Library `Greetings` exposing one class `Greetings` with one async method `SayHi`.
3. Add an ASP .NET web API `Greetings.Api` with a endpoint `/hello-world` calling `SayHi`.
4. Make sure everything works with a little `curl`.
5. For the web API
    1. Instrument the ASP .NET framework for metrics.
    2. Add a custom metric on the `hello-world` endpoint.
    3. Instrument the ASP .NET framework for tracing.
    4. **Add a custom span wrapping `SayHi` call. <- YOU ARE HERE** ✅
6. For the Class Library
    1. Add one log on `SayHi` following the `ILogger` interface. Run the app, see the console output with your new log.
    2. Disable the default Logger Providers in `Greetings.Api`. Run the app, it should work without logging spawning.
    3. Configure an OTL Logger Provider, run the app and check the console.

## Correction

1. Follow [`opentelemetry-dotnet` - Tracing Console Application](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/trace/getting-started-console/README.md) project documentation
2. Update `Program.cs` accordingly
3. `dotnet run --project Greetings.Api`
4. `curl localhost:<port>/hello-world` and watch the console, try to find the new ActivitySource.

## **Next commit**:

```bash
git switch --detach hands-on-1~3
```
