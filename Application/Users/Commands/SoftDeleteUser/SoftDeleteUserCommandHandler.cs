using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Shared;

namespace Application.Users.Commands.DeleteUser;

public class SoftDeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    :IRequestHandler<SoftDeleteUserCommand,Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Guid, cancellationToken);

        if (user is null)
            return Result.Failed("User not found");

        user.SoftDelete();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("User successfully deleted");
    }
}