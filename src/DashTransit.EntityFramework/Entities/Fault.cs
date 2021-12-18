// <copyright file="Fault.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.EntityFramework.Entities;

using System;
using Core.Domain;

public class Fault : Core.Domain.Fault
{
    private readonly Guid _rawMessageId;

    protected Fault(FaultId id, string exception, DateTime produced, EndpointId producedBy)
        : base(id, exception, produced, producedBy)
    {
    }

    public Fault(MessageId messageId, string exception, DateTime produced, EndpointId producedBy)
        : base(messageId, exception, produced, producedBy)
    {
        this._rawMessageId = messageId.Value;
    }

    public override MessageId MessageId => MessageId.From(this._rawMessageId);

    public new RawAudit Message { get; protected set; }
}