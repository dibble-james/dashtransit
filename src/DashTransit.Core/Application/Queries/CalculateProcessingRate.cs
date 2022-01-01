// <copyright file="CalculateMessageRate.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application.Queries;

public record CalculateProcessingRate : IRequest<TimeSpan>
{
    public class Handler : IRequestHandler<CalculateProcessingRate, TimeSpan>
    {
        private readonly ICalculateMessageRate _repository;

        public Handler(ICalculateMessageRate repository) => this._repository = repository;

        public Task<TimeSpan> Handle(CalculateProcessingRate request, CancellationToken cancellationToken)
        {
            return this._repository.ProcessingRate(TimeSpan.FromHours(1));
        }
    }
}