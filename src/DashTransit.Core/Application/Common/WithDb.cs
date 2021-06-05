// <copyright file="WithDb.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application.Common
{
    using System;
    using System.Threading.Tasks;
    using SqlKata.Execution;

    public static class With
    {
        public static Func<Func<QueryFactory, Task<T>>, Task<T>> Db<T>(Task<QueryFactory> queryFactory) => async (f) =>
        {
            var db = await queryFactory;

            return await f(db);
        };
    }
}
