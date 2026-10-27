using Application.Interfaces.Repositories.BaseInterfaces;
using Domain.Entities;

namespace Application.Interfaces.Repositories.BranchServiceInterfaces;

public interface IBranchServiceReadRepository:IReadRepository<BranchService,Guid>
{
    
}