// <copyright file="Message.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain
{
    using System;
    using DashTransit.Core.Domain.Common;

    public record MessageId : GuidIdentity<MessageId>
    {
        public MessageId(Guid Id)
            : base(Id)
        {
        }
    }

    public record CorrelationId : GuidIdentity<CorrelationId>
    {
        public CorrelationId(Guid Id)
            : base(Id)
        {
        }
    }

    public class Message : Aggregate<MessageId>
    {
        private readonly CorrelationId correlationId;
        private readonly DateTimeOffset timestamp;
        private readonly string content;
        private readonly string type;

        public Message(MessageId id)
            : base(id)
        {
        }

        public CorrelationId CorrelationId => this.correlationId;

        public DateTimeOffset Timestamp => this.timestamp;

        public string Content => this.content;

        public string Type => this.type;
    }
}
