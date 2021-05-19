// <copyright file="Aggregate.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class Aggregate<TIdentity> : Entity<TIdentity>
        where TIdentity : class
    {
        protected Aggregate(TIdentity id)
            : base(id)
        { }
    }
}