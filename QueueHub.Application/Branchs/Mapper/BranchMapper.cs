using System.Linq.Expressions;
using Application.Branchs.DTOs;
using Domain.Entities;

namespace Application.Branchs.Mapper;

public static class BranchMapper
{
    public static BranchDto ToDto(this Branch b) => b.ToDto(b.IsOpenNow());
    public static BranchDto ToDto(this Branch b, bool isOpen) => new(
        b.Id,
        b.Name,
        b.CityId,
        b.City.Name,
        b.Address,
        b.Description,
        b.PhoneNumber ?? string.Empty,
        b.City.Province.Name,
        isOpen);
    
    public static BranchDto ToDeletedDto(this Branch b) => b.ToDto(isOpen: false);
    
    
    public static BranchListDto ToListDto(this Branch b) => b.ToListDto(b.IsOpenNow());
    public static BranchListDto ToListDto(this Branch entity, bool isOpen) => new(
        entity.Id,
        entity.Name,
        entity.CityId,
        entity.City.Name,
        entity.Address,
        entity.City.Province.Name,
        isOpen);
    public static BranchListDto ToDeletedListDto(this Branch b) => b.ToListDto(isOpen: false);
}