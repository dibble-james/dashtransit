// <copyright file="Program.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

using System;
using System.Linq;
using DashTransit.App;
using DashTransit.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

static void RunHost(string[] args)
{
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        })
        .Build()
        .Run();
}

var task = args switch
{
    string[] a when (a.FirstOrDefault()?.Equals("migrate")) ?? false => Migrator.Migrate(Console.Out, Console.Error),
    _ => RunHost,
};

task(args);