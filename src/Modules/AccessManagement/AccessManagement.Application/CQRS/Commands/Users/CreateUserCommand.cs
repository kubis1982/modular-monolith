﻿namespace ModularMonolith.Modules.AccessManagement.CQRS.Commands.Users
{
    using ModularMonolith.Modules.AccessManagement.Domain.Users;
    using ModularMonolith.Shared.CQRS.Commands;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Creates a user.
    /// </summary>
    public record CreateUserCommand(string Email, string Password, string? FirstName, string? MiddleName, string? LastName) : EntityCommand
    {
        internal class CreateUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher) : EntityCommandHandler<CreateUserCommand>
        {
            public override async Task<EntityIdentityResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
            {
                UserPassword userPassword = UserPassword.Create(command.Password, passwordHasher);
                UserFullName fullName = UserFullName.Create(command.FirstName, command.MiddleName, command.LastName);
                UserEmail? userEmail = UserEmail.Of(command.Email);
                User user = User.Create(userEmail, userPassword, fullName);
                user = await userRepository.AddAsync(user, cancellationToken);
                return EntityIdentityResult.Create(user);
            }
        }
    }
}
