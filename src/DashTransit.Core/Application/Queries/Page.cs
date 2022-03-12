// <copyright file="Page.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application.Queries;

public record Page<T>(IEnumerable<T> Items, int Total);