// <copyright file="CalculateMessageRate.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application.Queries;

public record CalculateMessageRate(EndpointId? Endpoint = default) : IRequest<double>
{
    public class Handler : IRequestHandler<CalculateMessageRate, double>
    {
        private readonly ICalculateMessageRate _repository;

        public Handler(ICalculateMessageRate repository) => this._repository = repository;

        public Task<double> Handle(CalculateMessageRate request, CancellationToken cancellationToken)
        {
            return this._repository.MessageRate(TimeSpan.FromHours(1), request.Endpoint);
        }
    }
}