// <copyright file="Message.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using ValueOf;

    public class MessageId : ValueOf<Guid, MessageId> {}

    public class CorrelationId : ValueOf<Guid, CorrelationId> {}

    public class MessageType : ValueOf<string, MessageType> {}

    public class Message
    {
        private readonly List<Fault> faults;

        public Message(MessageId id, CorrelationId correlationId, DateTime timestamp, MessageType type, string content)
        {
            this.Id = id;
            this.CorrelationId = correlationId;
            this.Timestamp = timestamp;
            this.Content = content;
            this.Type = type;
            this.faults = new List<Fault>();
        }

        public MessageId Id { get; }

        public CorrelationId CorrelationId { get; }

        public DateTime Timestamp { get; }

        public MessageType Type { get; }

        public string Content { get; }

        public IReadOnlyCollection<Fault> Faults => this.faults;

        public void RegisterFault(Fault fault) => this.faults.Add(fault);
    }
}