﻿namespace Kubis1982.Modules.AccessManagement.Domain.Users.Exceptions
{
    using Kubis1982.Shared.Exceptions;

    public sealed class IncorrectEmailException(string? email) : AppException($"Incorrect email: {email}")
    {
    }
}
