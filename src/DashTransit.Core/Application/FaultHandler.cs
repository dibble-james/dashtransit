// <copyright file="FaultHandler.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application
{
    using System.Threading.Tasks;
    using DashTransit.Core.Domain;
    using DashTransit.Core.Domain.Common;
    using DashTransit.Core.Domain.Services;
    using MassTransit;

    public class FaultHandler : IConsumer<MassTransit.Fault>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly FaultService faultService;

        public FaultHandler(IUnitOfWork unitOfWork, FaultService faultService)
        {
            this.unitOfWork = unitOfWork;
            this.faultService = faultService;
        }

        public async Task Consume(ConsumeContext<MassTransit.Fault> context)
        {
            await this.faultService.RegisterFault(context.Message, new CorrelationId(context.CorrelationId!.Value));

            await this.unitOfWork.Commit();
        }
    }
}
