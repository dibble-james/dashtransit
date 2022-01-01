// <copyright file="CalculateFailureRate.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application.Queries;

public record CalculateFailureRate : IRequest<double>
{
    public class Handler : IRequestHandler<CalculateFailureRate, double>
    {
        private readonly ICalculateMessageRate _repository;

        public Handler(ICalculateMessageRate repository) => this._repository = repository;

        public Task<double> Handle(CalculateFailureRate request, CancellationToken cancellationToken)
        {
            return this._repository.FailureRate(TimeSpan.FromHours(1));
        }
    }
}