// <copyright file="Fault.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain
{
    public class Fault : Message
    {
        private readonly string exception;
        private readonly string stackTrace;
        private readonly string exceptionType;
        private readonly string source;

        public Fault(MessageId id)
            : base(id)
        {
        }

        public string Exception => this.exception;

        public string StackTrace => this.stackTrace;

        public string ExceptionType => this.exceptionType;

        public string Source => this.source;
    }
}
