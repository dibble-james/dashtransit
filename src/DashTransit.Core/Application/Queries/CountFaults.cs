// <copyright file="CountFaults.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application.Queries;

public record CountFaults : IRequest<int>
{
    public class Handler : IRequestHandler<CountFaults, int>
    {
        private readonly ICalculateMessageRate _repository;

        public Handler(ICalculateMessageRate repository) => this._repository = repository;

        public Task<int> Handle(CountFaults request, CancellationToken cancellationToken)
        {
            return this._repository.FaultCount(TimeSpan.FromHours(1));
        }
    }
}