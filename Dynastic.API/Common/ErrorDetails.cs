using System.Net;
using System.Text.Json;

namespace Dynastic.Common;

public class ErrorDetails
{
    public string Message { get; set; } = default!;
    public string? Trace { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }
}