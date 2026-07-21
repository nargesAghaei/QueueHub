namespace Application.Interfaces;

public interface ICurrentUserService
{
    string Name { get; }
    Guid UserId { get; }
    Guid? OrganizationId { get; }
    string? Role { get; }
}