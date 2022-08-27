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
            ErrorDetails? result;
            HttpStatusCode code;
            switch (e)
            {
                case NotFoundException:
                    result = new ErrorDetails() { Errors = new List<Error>() { new() { Message = e.Message } } };
                    code = HttpStatusCode.NotFound;
                    break;
                default:
                    result = new ErrorDetails() {
                        Errors = new List<Error>() { new() { Message = "Internal Server Error" } }
                    };
                    code = HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.StatusCode = (int) code;
            await context.Response.WriteAsJsonAsync(result);
        }
    }
}