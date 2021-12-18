// <copyright file="EndpointsHandler.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.EntityFramework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

public class EndpointsRepository : IEndpointRepository
{
    private readonly DashTransitContext context;

    public EndpointsRepository(DashTransitContext context) => this.context = context;

    public async Task<IEnumerable<EndpointId>> GetAll(CancellationToken cancellationToken)
    {
        var endpoints = await
            this.context.Audit.Select(x => x.SourceAddress)
                .Union(this.context.Audit.Select(x => x.InputAddress))
                .Distinct()
                .Where(x => x != null && !string.IsNullOrEmpty(x) && !x.EndsWith("temporary=true"))
                .Select(x => EndpointId.From(new Uri(x)))
                .ToListAsync(cancellationToken);

        return endpoints;
    }
}