﻿namespace Kubis1982.AccessManagement.Commands.Users
{
    using Kubis1982.AccessManagement.Domain.Users;
    using Kubis1982.Shared.CQRS.Commands;
    using Kubis1982.Shared.Security;
    using System.Threading;
    using System.Threading.Tasks;

    public record DeactivateUserCommand(int UserId) : UnitOfWorkCommand
    {
        internal class DeactivateUserCommandHandler(IUserRepository userRepository, IUserContext userContext) : UnitOfWorkCommandHandler<DeactivateUserCommand>
        {
            public override async Task Handle(DeactivateUserCommand command, CancellationToken cancellationToken)
            {
                User user = await userRepository.SingleAsync(UserSpec.ById(command.UserId), cancellationToken);
                User currentUser = await userRepository.SingleAsync(UserSpec.ById(userContext.Id), cancellationToken);
                user.Deactivate(currentUser);
            }
        }
    }
}
