using Domain.Exceptions;

namespace Domain.Entities;

public class Organization : BaseEntity<Guid>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string? Address { get; private set; }
    public Guid ManagerId { get; private set; }
    public bool IsActive { get; private set; }

    // private readonly List<Service> _services = new();
    // public IReadOnlyCollection<Service> Services => _services.AsReadOnly();

    private Organization() { } // EF Core

    public static Organization Create(string name, Guid managerId, string? description = null, string? address = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Organization name is required.");

        if (managerId == Guid.Empty)
            throw new DomainException("Organization must have a manager.");

        return new Organization
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Description = description,
            Address = address,
            ManagerId = managerId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateDetails(string name, string? description, string? address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Organization name is required.");

        Name = name.Trim();
        Description = description;
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException("Organization is already inactive.");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    // Services belong to the Organization - managed through it (Aggregate rule)
    // public Service AddService(string serviceName, string? description = null)
    // {
    //     if (_services.Any(s => s.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase)))
    //         throw new DomainException("A service with this name already exists in this organization.");
    //
    //     var service = Service.Create(serviceName, Id, description);
    //     _services.Add(service);
    //     return service;
    // }
    //
    // public void RemoveService(Guid serviceId)
    // {
    //     var service = _services.FirstOrDefault(s => s.Id == serviceId);
    //
    //     if (service is null)
    //         throw new DomainException("Service not found in this organization.");
    //
    //     _services.Remove(service);
    // }
}