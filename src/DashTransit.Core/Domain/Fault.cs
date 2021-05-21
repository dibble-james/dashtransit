// <copyright file="Fault.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain
{
    using System;

    public class Fault : Message
    {
        private readonly string exception;
        private readonly string stackTrace;
        private readonly string exceptionType;
        private readonly string source;

        public Fault(MessageId id, CorrelationId correlationId, DateTimeOffset timestamp, string content, string type, string exception, string stackTrace, string exceptionType, string source)
            : base(id, correlationId, timestamp, content, type)
        {
            this.exception = exception;
            this.stackTrace = stackTrace;
            this.exceptionType = exceptionType;
            this.source = source;
        }

        public string Exception => this.exception;

        public string StackTrace => this.stackTrace;

        public string ExceptionType => this.exceptionType;

        public string Source => this.source;
    }
}
