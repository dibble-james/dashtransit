// <copyright file="Fault.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain
{
    public class Fault
    {
        protected Fault(Endpoint raisedBy, string exception, string stackTrace, string exceptionType, string source)
        {
            this.RaisedBy = raisedBy;
            this.Exception = exception;
            this.StackTrace = stackTrace;
            this.ExceptionType = exceptionType;
            this.Source = source;
        }

        public Fault(Message message, Endpoint raisedBy, string exception, string stackTrace, string exceptionType, string source)
            : this(raisedBy, exception, stackTrace, exceptionType, source)
        {
            this.Message = message;
        }

        public Message? Message { get; }

        public Endpoint RaisedBy { get; }

        public string Exception { get; }

        public string StackTrace { get; }

        public string ExceptionType { get; }

        public string Source { get; }
    }
}
