// <copyright file="Fault.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain;

public class FaultId : ValueOf<int, FaultId>
{
}

public class Fault
{
    protected Fault(FaultId id, MessageId messageId, IEnumerable<ExceptionInfo> exceptions, DateTime produced,
        EndpointId producedBy)
        : this(messageId, exceptions, produced, producedBy)
    {
        this.Id = id;
    }

    public Fault(MessageId messageId, IEnumerable<ExceptionInfo> exceptions, DateTime produced, EndpointId producedBy)
    {
        this.MessageId = messageId;
        this.Exceptions = exceptions;
        this.Produced = produced;
        this.ProducedBy = producedBy;
    }

    public FaultId Id { get; } = null!;

    public virtual MessageId MessageId { get; }

    public virtual IEnumerable<ExceptionInfo> Exceptions { get; }

    public DateTime Produced { get; }

    public EndpointId ProducedBy { get; }

    public virtual IReadOnlyCollection<IRawAuditData> Messages { get; } = new List<IRawAuditData>();

    public IRawAuditData? Message => this.Messages.FirstOrDefault(Core.Domain.Message.IsProducer);
}