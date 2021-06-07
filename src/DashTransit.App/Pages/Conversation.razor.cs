// <copyright file="Message.razor.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.App.Pages
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Blazor.Diagrams.Core;
    using Blazor.Diagrams.Core.Models;
    using DashTransit.App.Components;
    using DashTransit.Core.Application;
    using DashTransit.Core.Domain;
    using MediatR;
    using Microsoft.AspNetCore.Components;
    using Core = DashTransit.Core.Application;

    public partial class Conversation
    {
        [Inject] private IMediator Mediator { get; init; }

        [Parameter]
        public Guid ConversationId { get; init; }

        private bool HasLoaded { get; set; }

        private Core.ConversationDetailsResponse Response { get; set; }

        private Diagram Diagram { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.Response = await this.Mediator.Send(new ConversationDetails(new CorrelationId(this.ConversationId)));
            this.HasLoaded = true;

            var options = new DiagramOptions();

            this.Diagram = new Diagram(options);
            this.Diagram.RegisterModelComponent<ConversationMessageModel, ConversationMessage>();

            var nodes = this.Response.Messages.Select(m => new ConversationMessageModel(m)).ToList();
            var edges = this.Response.Messages.Select(
                m => new LinkModel(
                    nodes.First(n => n.Message.MessageId == m.MessageId),
                    m.Parent is null ? null : nodes.FirstOrDefault(n => n.Message.MessageId == m.Parent.MessageId))).ToList();
            this.Diagram.Nodes.Add(nodes);
            this.Diagram.Links.Add(edges);

            this.StateHasChanged();
        }
    }
}
