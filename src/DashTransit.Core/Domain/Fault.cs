// <copyright file="Fault.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain;

public class FaultId : ValueOf<int, FaultId>
{
}

public class Fault
{
    protected Fault(FaultId id, MessageId messageId, string exception)
    {
        this.Id = id;
        this.MessageId = messageId;
        this.Exception = exception;
    }

    public Fault(MessageId messageId, string exception)
        : this(FaultId.From(0), messageId, exception)
    {
    }

    public FaultId Id { get; }

    public MessageId MessageId { get; }

    public string Exception { get; }
}