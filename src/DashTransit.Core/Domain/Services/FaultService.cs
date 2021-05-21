// <copyright file="FaultService.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using DashTransit.Core.Domain.Common;

    public class FaultService
    {
        private readonly IUnitOfWork context;

        public FaultService(IUnitOfWork context) => this.context = context;

        public async virtual Task RegisterFault(MassTransit.Fault receivedFault, CorrelationId correlationId)
        {
            var fault = new Fault(
                new MessageId(receivedFault.FaultedMessageId.GetValueOrDefault()),
                correlationId,
                receivedFault.Timestamp,
                "TODO",
                receivedFault.FaultMessageTypes.First(),
                receivedFault.Exceptions.First().Message,
                receivedFault.Exceptions.First().StackTrace,
                receivedFault.Exceptions.First().ExceptionType,
                receivedFault.Exceptions.First().Source);

            await this.context.Save<Fault, MessageId>(fault);
        }
    }
}
