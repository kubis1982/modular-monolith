﻿namespace Kubis1982.Modules.AccessManagement.Domain.Users
{
    using Kubis1982.Modules.AccessManagement.Domain;
    using Kubis1982.Modules.AccessManagement.Domain.Users.Events;
    using Kubis1982.Modules.AccessManagement.Domain.Users.Exceptions;
    using Kubis1982.Shared.Kernel;
    using Kubis1982.Shared.Kernel.Types;

    public sealed partial class User : DomainEntity<UserId, int, EntityType>, IAggregateRoot
    {
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        internal UserEmail Email { get; private set; }

        /// <summary>
        /// Gets the password of the user.
        /// </summary>
        internal UserPassword Password { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active.
        /// </summary>
        internal bool IsActive { get; private set; } = false;

        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        internal UserFullName FullName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private User()
        {
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="fullName">The full name of the user.</param>
        private User(UserEmail email, UserPassword password, UserFullName fullName) : this()
        {
            Email = email;
            Password = password;
            FullName = fullName;            

            AddEvent(new UserCreatedEvent(this, email, fullName));
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="email">The email of the user (optional).</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="fullName">The full name of the user.</param>
        /// <returns>The created user.</returns>
        public static User Create(UserEmail email, UserPassword password, UserFullName fullName)
        {
            return new User(email, password, fullName);
        }

        /// <summary>
        /// Changes the password of the user.
        /// </summary>
        /// <param name="password">The new password.</param>
        public void ChangePassword(UserPassword password)
        {
            if (Password.Equals(password))
            {
                throw new PasswordsCannotBeTheSameException();
            }
            Password = password;
            AddEvent(new UserPasswordChangedEvent(this));
        }

        /// <summary>
        /// Updates the user information.
        /// </summary>
        /// <param name="userFullName">The new full name.</param>
        public void Update(UserFullName userFullName)
        {
            FullName = userFullName;
            
            AddEvent(new UserUpdatedEvent(this, userFullName));
        }

        /// <summary>
        /// Activates the user.
        /// </summary>
        /// <param name="currentUser">The current user performing the action.</param>
        public void Activate(User currentUser)
        {
            if (currentUser == this)
            {
                throw new ActionOnCurrentUserException();
            }
            if (!IsActive)
            {
                IsActive = true;
                AddEvent(new UserActivatedEvent(this, currentUser));
            }
        }

        /// <summary>
        /// Deactivates the user.
        /// </summary>
        /// <param name="currentUser">The current user performing the action.</param>
        public void Deactivate(User currentUser)
        {
            if (currentUser == this)
            {
                throw new ActionOnCurrentUserException();
            }

            if (IsActive)
            {
                IsActive = false;
                AddEvent(new UserDeactivatedEvent(this, currentUser));
            }
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="currentUser">The current user performing the action.</param>
        public void Delete(User currentUser)
        {
            if (currentUser == this)
            {
                throw new ActionOnCurrentUserException();
            }
            if (Id == UserId.Administrator)
            {
                throw new DeletingAdministratorException();
            }
            AddEvent(new UserDeletedEvent(this, currentUser));
        }

        /// <summary>
        /// Gets the administrator user.
        /// </summary>
        internal static User Administrator => new(UserEmail.Of("kubis1982@kubis1982.com"), UserPassword.Of("8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918"), UserFullName.Create("Administrator")) { Id = UserId.Administrator };
    }
}
