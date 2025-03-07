# Observability-102 Second Hands-on

## Getting Started

First of all, run the fully configured observability back-end available through `docker-compose`:
```bash
docker-compose up
```

This environment is composed of:
- a [Jaeger](https://www.jaegertracing.io/) back-end for distributed traces.
- a [Prometheus](https://prometheus.io/) back-end for metrics.

Both environments UI are avaibable at:
- **Jaeger**: `localhost:16686`
- **Prometheus**: `localhost:9090`

> If you are curious, you can dive into [`docker-compose.yml`](./docker-compose.yml), [`otel-collector-conf.yml`](./otel-collector-config.yml) and [`prometheus.yml`](./prometheus.yml) to know more about configuration, network plumbing and services exposition.

## Instructions

You have an API that is correctly instrumented but currently, your exporters are **only configured to point towards the Console**. It is suitable for development purposes but it's quite hard to read your telemetry as is and you want to leverage observability back-ends capabilities.

Your loved Infrastructure team configured for you an `OTL collector`. The have thoroughly documented the infrastructure.

**Application Layer (Layer 7)**:
- Telemetry Communication Protocol: `OTLP`
- Communication Protocol: `gRPC`

**Transport Layer (Layer 4)**:
- Port: `4317`

**Network Layer (Layer 3)**:
- IP address: `localhost`

Also, you have documentation on how to configure and OTLP Exporter at [OTLP Exporter for OpenTelemetry .NET](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/README.md).

Configure the API to export **metrics** and **traces** to, respectively, **Prometheus** and **Jaeger**. Try to catch you telemetry data into each UI.

You can run the API with:
```bash
dotnet run --project Greetings.Api
```

Try to call the `/hello-world` endpoint with:
```bash
curl localhost:<port>/hello-world
```

## Correction

Go to
```bash
git switch --detach hands-on-2~1
```
to see correction
