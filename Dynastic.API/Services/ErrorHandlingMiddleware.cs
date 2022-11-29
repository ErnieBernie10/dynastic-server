using Ardalis.GuardClauses;
using Dynastic.Common;
using Dynastic.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Azure.Cosmos;
using System.Net;
using System.Text.Json;

namespace Dynastic.API.Services;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            HttpStatusCode code;
            switch (e)
            {
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.StatusCode = (int) code;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                Message = e.Message,
                StatusCode = code,
                // TODO: Remove stacktrace for production
                Trace = e.StackTrace,
            }.ToString());
        }
    }
}