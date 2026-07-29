using FluentValidation;
using MediatR;

namespace QueueHub.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {

        if (!validators.Any())
        {
            return await next();
        }


        var context = new ValidationContext<TRequest>(request);


        var validationResults = await Task.WhenAll(
            validators.Select(
                validator => 
                    validator.ValidateAsync(
                        context,
                        cancellationToken))
        );


        var errors = validationResults
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();


        if (errors.Count != 0)
        {
            throw new ValidationException(errors);
        }


        return await next();
    }
}