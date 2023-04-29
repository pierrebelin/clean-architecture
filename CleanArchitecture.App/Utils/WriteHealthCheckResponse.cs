using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace CleanArchitecture.App.Utils;

internal static class HealthCheckExtensions
{
    internal static Task WriteHealthCheckResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";
        return context.Response.WriteAsync(JsonConvert.SerializeObject(healthReport));
    }
}

