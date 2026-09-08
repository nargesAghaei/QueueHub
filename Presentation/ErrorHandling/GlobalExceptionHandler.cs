using System.Net;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace QueueHub.ErrorHandling;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var statusCode = ResolveStatusCode(exception);

        await LogExceptionAsync(
            httpContext,
            exception,
            statusCode);

        if (exception is ValidationException validationException)
        {
            await WriteValidationProblemAsync(
                httpContext,
                validationException,
                cancellationToken);

            return true;
        }

        await WriteProblemAsync(
            httpContext,
            statusCode,
            ResolveDetail(exception, statusCode),
            cancellationToken);

        return true;
    }

    private static HttpStatusCode ResolveStatusCode(
        Exception exception) =>
        exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            DomainException => HttpStatusCode.BadRequest,
            ArgumentException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            KeyNotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };

    private static string ResolveDetail(
        Exception exception,
        HttpStatusCode statusCode) =>
        // statusCode == HttpStatusCode.InternalServerError
        //     ? "An unexpected error occurred."
            exception.Message;

    private async Task LogExceptionAsync(
        HttpContext httpContext,
        Exception exception,
        HttpStatusCode statusCode)
    {
        logger.LogError(
            exception,
            exception.Message);

        try
        {
            var exceptionLogRepository =
                httpContext.RequestServices
                    .GetRequiredService<IExceptionLogWriteRepository>();

            var log = ExceptionLog.CreateByException(
                exception,
                (int)statusCode,
                httpContext.Request.Path,
                httpContext.Request.Method);

            await exceptionLogRepository.AddAsync(
                log,
                httpContext.RequestAborted);
        }
        catch (Exception loggingException)
        {
            logger.LogError(
                loggingException,
                "Failed to persist exception log to MongoDB for {ExceptionType}",
                exception.GetType().Name);
        }
    }

    private static Task WriteValidationProblemAsync(
        HttpContext context,
        ValidationException validationException,
        CancellationToken cancellationToken)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var errors = validationException.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(error => error.ErrorMessage)
                    .Distinct()
                    .ToArray());

        var problem = new
        {
            status = StatusCodes.Status400BadRequest,
            title = "Validation Failed",
            errors
        };

        return context.Response.WriteAsJsonAsync(
            problem,
            cancellationToken);
    }

    private static Task WriteProblemAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string detail,
        CancellationToken cancellationToken)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problem = new
        {
            status = (int)statusCode,
            title = statusCode.ToString(),
            detail
        };

        return context.Response.WriteAsJsonAsync(
            problem,
            cancellationToken);
    }
}