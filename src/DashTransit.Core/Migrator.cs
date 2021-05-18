// <copyright file="Migrator.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core
{
    using System;
    using System.IO;
    using System.Linq;
    using DbUp;

    public static class Migrator
    {
        public static Action<string[]> Migrate(TextWriter output, TextWriter error) => args =>
        {
            output.WriteLine("Migrating Dashtransit Database...");

            var connectionString = args.Skip(1).FirstOrDefault();

            if (connectionString is null)
            {
                error.WriteLine("Migrate command requires a connection string");
                return;
            }

            EnsureDatabase.For.SqlDatabase(connectionString);

            var result = DeployChanges
                .To.SqlDatabase(connectionString)
                .LogToConsole()
                .WithScriptsEmbeddedInAssembly(typeof(Hook).Assembly)
                .JournalToSqlTable("dbo", "__Migrations")
                .Build()
                .PerformUpgrade();

            if (result.Successful)
            {
                output.WriteLine("Migrations Complete");
            }
        };
    }
}
