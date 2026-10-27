using Application.Interfaces.Repositories.BaseInterfaces;
using Domain.Entities;

namespace Application.Interfaces.Repositories.BranchServiceInterfaces;

public interface IBranchServiceWriteRepository:IWriteRepository<BranchService,Guid>
{
    
}