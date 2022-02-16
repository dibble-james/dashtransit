// <copyright file="Fault.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.EntityFramework.Entities;

using System;
using System.Collections.Generic;
using Core.Domain;
using MassTransit;

public class Fault : Core.Domain.Fault
{
    protected Fault(FaultId id, MessageId messageId, IEnumerable<ExceptionInfo> exceptions, DateTime produced, EndpointId producedBy)
        : base(id, messageId, exceptions, produced, producedBy)
    {
    }

    public Fault(MessageId messageId, IEnumerable<ExceptionInfo> exceptions, DateTime produced, EndpointId producedBy, IReadOnlyCollection<IRawAuditData> messages)
        : base(messageId, exceptions, produced, producedBy)
    {
        this.Messages = messages;
    }

    public Fault(FaultId id, MessageId messageId, IEnumerable<ExceptionInfo> exceptions, DateTime produced, EndpointId producedBy, IReadOnlyCollection<IRawAuditData> messages)
        : this(id, messageId, exceptions, produced, producedBy)
    {
        this.Messages = messages;
    }

    public override IReadOnlyCollection<IRawAuditData> Messages { get; } = null!;
}