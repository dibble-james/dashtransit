// <copyright file="FaultHandler.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application
{
    using System.Threading.Tasks;
    using DashTransit.Core.Domain;
    using DashTransit.Core.Domain.Services;
    using MassTransit;

    public class FaultHandler : IConsumer<MassTransit.Fault>
    {
        private readonly FaultService faultService;

        public FaultHandler(FaultService faultService)
        {
            this.faultService = faultService;
        }

        public async Task Consume(ConsumeContext<MassTransit.Fault> context)
        {
            await this.faultService.RegisterFault(context.Message, new CorrelationId(context.CorrelationId!.Value));
        }
    }
}
