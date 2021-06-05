// <copyright file="UnitOfWork.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using DashTransit.Core.Domain.Common;

    public class UnitOfWork : IUnitOfWork
    {
        public Task Save<T, TId>(T entity)
            where T : Aggregate<TId>
            where TId : class
        {
            throw new NotImplementedException();
        }
    }
}
