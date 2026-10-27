using System.Diagnostics;
using System.Net;
using Application.Interfaces.Repositories.ExceptionLogInterfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace QueueHub.ErrorHandling;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // خود کاربر درخواست را لغو کرده، خطا نیست
        if (exception is OperationCanceledException &&
            httpContext.RequestAborted.IsCancellationRequested)
            return true;

        var statusCode = ResolveStatusCode(exception);

        await LogExceptionAsync(httpContext, exception, statusCode);

        var problem = exception is ValidationException validationException
            ? CreateValidationProblem(validationException)
            : new ProblemDetails
            {
                Status = (int)statusCode,
                Title = ReasonPhrases.GetReasonPhrase((int)statusCode),
                Detail = ResolveDetail(exception, statusCode)
            };

        problem.Extensions["traceId"] =
            Activity.Current?.Id ?? httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = problem.Status ?? (int)statusCode;

        var written = await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception
        });

        if (!written)
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    private static HttpStatusCode ResolveStatusCode(Exception exception) =>
        exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            DomainException => HttpStatusCode.BadRequest,
            ArgumentException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            KeyNotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };

    // فقط پیام خودمان (DomainException) به کاربر می‌رسد
    private static string ResolveDetail(Exception exception, HttpStatusCode statusCode) =>
        exception is DomainException
            ? exception.Message
            : statusCode switch
            {
                HttpStatusCode.BadRequest => "درخواست نامعتبر است.",
                HttpStatusCode.Unauthorized => "برای انجام این کار باید وارد شوید.",
                HttpStatusCode.NotFound => "مورد درخواستی پیدا نشد.",
                _ => "خطای غیرمنتظره‌ای رخ داد."
            };

    private static ValidationProblemDetails CreateValidationProblem(
        ValidationException validationException)
    {
        var errors = validationException.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(error => error.ErrorMessage)
                    .Distinct()
                    .ToArray());

        return new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred."
        };
    }

    private async Task LogExceptionAsync(
        HttpContext httpContext,
        Exception exception,
        HttpStatusCode statusCode)
    {
        if ((int)statusCode < 500)
        {
            logger.LogWarning(
                "Handled {ExceptionType} ({StatusCode}) on {Method} {Path}: {Message}",
                exception.GetType().Name,
                (int)statusCode,
                httpContext.Request.Method,
                httpContext.Request.Path,
                exception.Message);
            return;
        }

        logger.LogError(
            exception,
            "Unhandled exception on {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path);

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

            await exceptionLogRepository.AddAsync(log, CancellationToken.None);
        }
        catch (Exception loggingException)
        {
            logger.LogError(
                loggingException,
                "Failed to persist exception log to MongoDB for {ExceptionType}",
                exception.GetType().Name);
        }
    }
}