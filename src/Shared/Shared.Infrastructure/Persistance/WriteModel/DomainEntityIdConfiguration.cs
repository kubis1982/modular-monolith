﻿namespace ModularMonolith.Shared.Persistance.WriteModel
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using ModularMonolith.Shared.Kernel.Types;
    using System;

    public abstract class DomainEntityIdConfiguration<TEntity, TEntityId> : EntityTypeConfiguration<TEntity>
        where TEntity : DomainEntity<TEntityId, int>
        where TEntityId : EntityId<int>, new()
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);
            AddIdentityField(builder);
            AddCreatedField(builder);
            AddModifiedField(builder);
            AddKey(builder);
        }

        public virtual void AddIdentityField(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(n => n.Id).IsRequired(true)
                .HasConversion(n => n.Id, n => new TEntityId { Id = n })
                .ValueGeneratedOnAdd()
                .HasColumnOrder(1);
        }

        public virtual void AddCreatedField(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property<DateTime?>(ShadowProperties.CreatedOn).HasColumnOrder(2)
                .ValueGeneratedOnAdd().HasDefaultValueSql("NOW()");
            builder.Property<int?>(ShadowProperties.CreatedBy).HasColumnOrder(3);
        }

        public virtual void AddModifiedField(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property<DateTime?>(ShadowProperties.ModifiedOn).HasColumnOrder(4);
            builder.Property<int?>(ShadowProperties.ModifiedBy).HasColumnOrder(5);
        }

        public virtual void AddKey(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(n => n.Id);
        }
    }
}
