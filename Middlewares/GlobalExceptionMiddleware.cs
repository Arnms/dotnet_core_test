using System.Net;
using dotnet_server_test.Models.Dtos;

namespace dotnet_server_test.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    public async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = context.Response;
        ResponseModel exceptionModel = new();

        switch (exception)
        {
            case ApplicationException ex:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                exceptionModel.Code = (int)HttpStatusCode.BadRequest;
                exceptionModel.Message = "Application Exception Occured.";
                break;
            case FileNotFoundException ex:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                exceptionModel.Code = (int)HttpStatusCode.NotFound;
                exceptionModel.Message = "The requested resource is not found.";
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                exceptionModel.Code = (int)HttpStatusCode.InternalServerError;
                exceptionModel.Message = "Internal Server Error.";
                break;
        }

        await context.Response.WriteAsJsonAsync(exceptionModel);
    }
}