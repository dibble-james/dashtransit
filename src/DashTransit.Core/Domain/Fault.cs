// <copyright file="Fault.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain;

public class FaultId : ValueOf<int, FaultId>
{
}

public class Fault
{
    protected Fault(FaultId id, string exception, DateTime produced, EndpointId producedBy)
        : this(default(MessageId)!, exception, produced, producedBy)
    {
        this.Id = id;
    }

    public Fault(MessageId messageId, string exception, DateTime produced, EndpointId producedBy)
    {
        this.MessageId = messageId;
        this.Exception = exception;
        this.Produced = produced;
        this.ProducedBy = producedBy;
    }

    public FaultId Id { get; } = null!;

    public virtual MessageId MessageId { get; }

    public string Exception { get; }

    public DateTime Produced { get; }

    public EndpointId ProducedBy { get; }

    public IRawAuditData? Message { get; protected set; }
}