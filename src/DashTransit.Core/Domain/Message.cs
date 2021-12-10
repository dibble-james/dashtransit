namespace DashTransit.Core.Domain;

using System;
using ValueOf;

public class MessageId : ValueOf<Guid, MessageId> {}

public class AuditId : ValueOf<int, AuditId> {}