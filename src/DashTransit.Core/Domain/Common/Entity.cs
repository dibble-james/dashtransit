// <copyright file="Aggregate.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain.Common
{
    public abstract class Entity<TIdentity>
        where TIdentity : class
    {
        protected Entity(TIdentity id) => this.Id = id;

        public TIdentity Id { get; }
    }
}