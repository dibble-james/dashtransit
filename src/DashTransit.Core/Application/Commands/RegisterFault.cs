// <copyright file="RegisterFault.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application.Commands;

public record RegisterFault(MassTransit.Fault Fault) : IRequest
{
    public class Handler : IRequestHandler<RegisterFault>
    {
        private readonly IRepositoryBase<Fault> repository;

        public Handler(IRepositoryBase<Fault> repository) => this.repository = repository;

        public async Task<Unit> Handle(RegisterFault request, CancellationToken cancellationToken)
        {
            await this.repository.AddAsync(
                new Fault(MessageId.From(request.Fault.FaultedMessageId!.Value), request.Fault.Exceptions.First().Message),
                cancellationToken);

            return Unit.Value;
        }
    }
}