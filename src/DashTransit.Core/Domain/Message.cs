namespace DashTransit.Core.Domain;

using System;
using ValueOf;

public class MessageId : ValueOf<Guid, MessageId> {}

public class Message
{
    public static Func<IRawAuditData, bool> IsProducer =>
        message => new[] {"Publish", "Send"}.Contains(message.ContextType);
}

public class AuditId : ValueOf<int, AuditId> {}