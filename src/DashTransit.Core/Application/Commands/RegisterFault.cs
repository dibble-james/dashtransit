// <copyright file="RegisterFault.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application.Commands;

using DashTransit.Core.Domain;

public record RegisterFault(MassTransit.Fault Fault, EndpointId Endpoint) : IRequest
{
    public class Handler : IRequestHandler<RegisterFault>
    {
        private readonly IRepositoryBase<Fault> repository;

        public Handler(IRepositoryBase<Fault> repository) => this.repository = repository;

        public async Task<Unit> Handle(RegisterFault request, CancellationToken cancellationToken)
        {
            await this.repository.AddAsync(
                new Fault(
                    MessageId.From(request.Fault.FaultedMessageId!.Value),
                    request.Fault.Exceptions.First().Message,
                    request.Fault.Timestamp,
                    request.Endpoint),
                cancellationToken);

            return Unit.Value;
        }
    }

    public class Consumer : IConsumer<MassTransit.Fault>
    {
        private readonly IMediator mediator;

        public Consumer(IMediator mediator) => this.mediator = mediator;

        public async Task Consume(ConsumeContext<MassTransit.Fault> context)
        {
            await this.mediator.Send(new RegisterFault(context.Message, EndpointId.From(context.SourceAddress)));
        }
    }
}