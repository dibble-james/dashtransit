// <copyright file="Endpoints.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application.Queries;

public class Endpoints : IRequest<IEnumerable<EndpointId>>
{
    public class Handler : IRequestHandler<Endpoints, IEnumerable<EndpointId>>
    {
        private readonly IEndpointRepository repository;

        public Handler(IEndpointRepository repository) => this.repository = repository;

        public Task<IEnumerable<EndpointId>> Handle(Endpoints request, CancellationToken cancellationToken)
        {
            return this.repository.GetAll(cancellationToken);
        }
    }
}