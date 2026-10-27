using Application.Interfaces.Repositories.BranchInterfaces;
using Application.Interfaces.Repositories.BranchServiceInterfaces;
using Domain.Entities;
using Infrastructure.Persistence.PostgreSql.EfCore.Repositories.BaseRepo;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Repositories.BranchServiceRepo;

public class BranchServiceReadRepository
    :BaseReadRepository<BranchService,Guid>,
        IBranchServiceReadRepository
{
    
}