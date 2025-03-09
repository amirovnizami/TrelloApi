using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Trello.Common.GlobalResponses;
using Trello.Common.GlobalResponses.Generics;
using Trello.Repository.Common.Exception;

namespace Trello.WebUi.Midlleware;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAysnc(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            switch (error)
            {
                case BadRequestException :
                    var message = new List<string>() { error.Message };
                    await WriteError(context,HttpStatusCode.BadRequest, message);
                    break;
                default:
                    message = new List<string>() { error.Message };
                    await WriteError(context,HttpStatusCode.InternalServerError, message);
                    break;
            }
        }
    }
    public static async Task WriteError(HttpContext context, HttpStatusCode statusCode, List<string> messages)
    {
        context.Response.Clear();
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        var json = JsonSerializer.Serialize(new Result(messages));
        await context.Response.WriteAsync(json);
    }
}