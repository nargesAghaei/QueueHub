using Application.Interfaces.Repositories.BranchServiceInterfaces;
using Domain.Entities;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Repositories.BranchServiceRepo;

public class BranchServiceWriteRepository
    :BaseWriteRepository<BranchService,Guid>,
        IBranchServiceWriteRepository
{
    
}