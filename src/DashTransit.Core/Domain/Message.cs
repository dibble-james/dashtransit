namespace DashTransit.Core.Domain;

using System;
using System.Linq.Expressions;
using ValueOf;

public class MessageId : ValueOf<Guid, MessageId> {}

public class Message
{
    public static Expression<Func<IRawAuditData, bool>> IsProducerSpec =>
        message => new[] {"Publish", "Send"}.Contains(message.ContextType);

    public static Func<IRawAuditData, bool> IsProducer =>
        message => new[] {"Publish", "Send"}.Contains(message.ContextType);
}

public class AuditId : ValueOf<int, AuditId> {}