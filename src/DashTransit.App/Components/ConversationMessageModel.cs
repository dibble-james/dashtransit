// <copyright file="Class.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.App.Components
{
    using Blazor.Diagrams.Core.Geometry;
    using Blazor.Diagrams.Core.Models;
    using DashTransit.Core.Application;

    public class ConversationMessageModel : NodeModel
    {
        public ConversationMessageModel(ConversationDetailsResponse.Message message, Point poistion = null)
            : base(poistion)
        {
            this.Message = message;
            this.AddPort(PortAlignment.Top);
            this.AddPort(PortAlignment.Right);
            this.AddPort(PortAlignment.Bottom);
            this.AddPort(PortAlignment.Left);
        }

        public ConversationDetailsResponse.Message Message { get; }
    }
}
