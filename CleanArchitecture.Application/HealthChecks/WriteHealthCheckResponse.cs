using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanArchitecture.Application.HealthChecks;

internal static class HealthCheckExtensions
{
    internal static Task WriteHealthCheckResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";
        return context.Response.WriteAsync(JsonSerializer.Serialize(healthReport));
    }
}

