// <copyright file="IUnitOfWork.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain.Common
{
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        Task Save<T, TId>(T entity)
            where T : Aggregate<TId>
            where TId : class;

        Task Commit();
    }
}