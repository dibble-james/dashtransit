// <copyright file="DbConnectionFactory.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Infrastructure
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using SqlKata.Compilers;
    using SqlKata.Execution;

    public static class DbConnectionFactory
    {
        private static readonly Compiler Compiler = new SqlServerCompiler();

        public static Func<IServiceProvider, Task<QueryFactory>> Open(string connectionString) => async _ =>
        {
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            var db = new QueryFactory(connection, Compiler);
            db.Logger = compiled => Console.WriteLine(compiled.ToString());

            return db;
        };
    }
}
