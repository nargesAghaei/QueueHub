namespace Application.Interfaces;

public interface ICurrentUserService
{
    string Name { get; }
    Guid Id { get; }
    Guid? OrganizationId { get; }
    string? Role { get; }
}