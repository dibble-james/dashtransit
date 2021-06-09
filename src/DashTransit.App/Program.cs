// <copyright file="Program.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

using System;
using System.Linq;
using DashTransit.App;
using DashTransit.Core.Infrastructure;

var task = args.FirstOrDefault() switch
{
    "migrate" => Migrator.Migrate(Console.Out, Console.Error),
    _ => WebApp.Run,
};

task(args);