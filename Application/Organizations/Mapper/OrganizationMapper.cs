using Application.Organizations.DTOs;
using Domain.Entities;

namespace Application.Organizations.Mapper;

public static class OrganizationMapper
{
    public static OrganizationDto ToDto(this Organization organization)
    {
        return new OrganizationDto
        {
            Name = organization.Name.Value,
            City = organization.City,
            Address =  organization.Address,
            Description =  organization.Description,
            PhoneNumber = organization.PhoneNumber,
            Website = organization.Website,
            LogoUrl = organization.LogoUrl
        };
    }
}