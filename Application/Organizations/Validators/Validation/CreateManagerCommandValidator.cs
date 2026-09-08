using Application.Organizations.Commands.CreateManager;
using Application.Organizations.Validators.Messages;
using FluentValidation;

namespace Application.Organizations.Validators.Validation;

public class CreateManagerCommandValidator
    : AbstractValidator<CreateManagerCommand>
{
    public CreateManagerCommandValidator()
    {
        // Manager
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(ManagerMessages.FirstNameRequired)
            .MaximumLength(100)
            .WithMessage(ManagerMessages.FirstNameTooLong);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(ManagerMessages.LastNameRequired)
            .MaximumLength(100)
            .WithMessage(ManagerMessages.LastNameTooLong);

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage(ManagerMessages.UserNameRequired)
            .MaximumLength(100)
            .WithMessage(ManagerMessages.UserNameTooLong);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ManagerMessages.PasswordRequired)
            .MinimumLength(8)
            .WithMessage(ManagerMessages.PasswordTooShort);

        RuleFor(x => x.Mobile)
            .NotEmpty()
            .WithMessage(ManagerMessages.MobileRequired);

        RuleFor(x => x.EmailManager)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.EmailManager))
            .WithMessage(ManagerMessages.EmailInvalid);


        // Organization
        RuleFor(x => x.OrganizationName)
            .NotEmpty()
            .WithMessage(OrganizationMessages.NameRequired)
            .MaximumLength(200)
            .WithMessage(OrganizationMessages.NameTooLong);

        RuleFor(x => x.OrganizationCode)
            .NotEmpty()
            .WithMessage(OrganizationMessages.CodeRequired)
            .MaximumLength(50)
            .WithMessage(OrganizationMessages.CodeTooLong);

        RuleFor(x => x.OrganizationDescription)
            .NotEmpty()
            .WithMessage(OrganizationMessages.DescriptionRequired)
            .MaximumLength(1000)
            .WithMessage(OrganizationMessages.DescriptionTooLong);

        RuleFor(x => x.OrganizationPhoneNumber)
            .NotEmpty()
            .WithMessage(OrganizationMessages.PhoneRequired);

        RuleFor(x => x.OrganizationEmail)
            .NotEmpty()
            .WithMessage(OrganizationMessages.EmailRequired)
            .EmailAddress()
            .WithMessage(OrganizationMessages.EmailInvalid);

        RuleFor(x => x.OrganizationWebsite)
            .Must(BeValidUrl)
            .When(x => !string.IsNullOrWhiteSpace(x.OrganizationWebsite))
            .WithMessage(OrganizationMessages.WebsiteInvalid);

        RuleFor(x => x.OrganizationAddress)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.OrganizationAddress))
            .WithMessage(OrganizationMessages.AddressTooLong);

        RuleFor(x => x.OrganizationCity)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.OrganizationCity))
            .WithMessage(OrganizationMessages.CityTooLong);

        RuleFor(x => x.OrganizationCategoryId)
            .NotEmpty()
            .WithMessage(OrganizationMessages.CategoryRequired);

        RuleFor(x => x.OrganizationLogoUrl)
            .Must(BeValidUrl)
            .When(x => !string.IsNullOrWhiteSpace(x.OrganizationLogoUrl))
            .WithMessage(OrganizationMessages.LogoUrlInvalid);
    }

    private static bool BeValidUrl(string? value)
    {
        return Uri.TryCreate(
            value,
            UriKind.Absolute,
            out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp ||
                uri.Scheme == Uri.UriSchemeHttps);
    }
}