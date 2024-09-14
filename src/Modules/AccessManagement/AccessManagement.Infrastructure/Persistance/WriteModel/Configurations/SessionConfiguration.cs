﻿namespace ModularMonolith.Modules.AccessManagement.Persistance.WriteModel.Configurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    internal class SessionConfiguration : DomainEntityTypeConfiguration<Session, SessionId, EntityType>
    {
        protected override void OnConfigure(EntityTypeBuilder<Session> builder)
        {
            builder.Property(n => n.ExpiryDate).IsRequired(true);
            builder.OwnsOne(n => n.RefreshToken, n => {
                n.Property(m => m!.Token).HasMaxLength(UserRestriction.RefreshTokenLength).HasColumnName("RefreshToken");
                n.Property(m => m!.ExpiryTime).HasColumnName("RefreshTokenExpiryDate");
            });
            builder.HasOne(n => n.Killer).WithMany().HasForeignKey("KilledBy").OnDelete(DeleteBehavior.Restrict);
            builder.Property<UserId>("KilledBy");

            builder.Navigation(n => n.Killer).AutoInclude();
        }

        public override void AddCreatedField(EntityTypeBuilder<Session> builder)
        {
            builder.Property<DateTime?>(ShadowProperties.CreatedOn).HasColumnOrder(2);            
            builder.Property<UserId>(ShadowProperties.CreatedBy).HasColumnOrder(3).IsRequired(true);
        }
    }
}
