using Application.Organizations.DTOs;
using Domain.Entities;

namespace Application.Organizations.Mapper;

public static class OrganizationMapper
{
    public static OrganizationDto ToDto(this Organization organization)
    {
        return new OrganizationDto
        {
            Id =  organization.Id,
            Name = organization.Name.Value,
            City = organization.CityId,
            Address =  organization.Address,
            Description =  organization.Description,
            PhoneNumber = organization.PhoneNumber,
            Website = organization.Website,
            LogoUrl = organization.LogoUrl
        };
    }
}