namespace DashTransit.Core.Domain;

using System;
using System.Collections.Generic;

public interface IRawAuditData
{
    int AuditRecordId { get; }
    Guid? MessageId { get; }
    Guid? ConversationId { get; }
    Guid? CorrelationId { get; }
    Guid? InitiatorId { get; }
    Guid? RequestId { get; }
    DateTime? SentTime { get; }
    string SourceAddress { get; }
    string DestinationAddress { get; }
    string ResponseAddress { get; }
    string FaultAddress { get; }
    string InputAddress { get; }
    string ContextType { get; }
    string MessageType { get; }
    Dictionary<string, string> Custom { get; }
    Dictionary<string, string> Headers { get; }
    object Message { get; }
}