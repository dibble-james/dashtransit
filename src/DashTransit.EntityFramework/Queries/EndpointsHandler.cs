// <copyright file="EndpointsHandler.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.EntityFramework.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Application.Queries;
using Core.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class EndpointsHandler : IRequestHandler<Endpoints, IEnumerable<EndpointId>>
{
    private readonly DashTransitContext context;

    public EndpointsHandler(DashTransitContext context) => this.context = context;

    public async Task<IEnumerable<EndpointId>> Handle(Endpoints request, CancellationToken cancellationToken)
    {
        var endpoints = await this.context.Audit
            .SelectMany(x => new[] {x.SourceAddress, x.DestinationAddress, x.InputAddress})
            .Distinct()
            .Where(x => x != null)
            .Select(x => EndpointId.From(new Uri(x)))
            .ToListAsync(cancellationToken);

        return endpoints;
    }
}