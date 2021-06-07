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
    using Microsoft.Msagl.Core.Geometry;
    using Microsoft.Msagl.Core.Geometry.Curves;
    using Microsoft.Msagl.Core.Layout;
    using Microsoft.Msagl.Miscellaneous;
    using Microsoft.Msagl.Prototype.Ranking;
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

            var graphModel = new GeometryGraph();
            foreach (var node in this.Response.Messages)
            {
                graphModel.Nodes.Add(new Node(CurveFactory.CreateRectangle(400, 200, new Point()), node.MessageId));
            }

            foreach (var node in this.Response.Messages.Where(n => n.Parent is not null))
            {
                graphModel.Edges.Add(
                    new Edge(
                        graphModel.FindNodeByUserData(node.MessageId),
                        graphModel.FindNodeByUserData(node.Parent.MessageId)));
            }

            LayoutHelpers.CalculateLayout(graphModel, new RankingLayoutSettings(), null);

            var options = new DiagramOptions();

            this.Diagram = new Diagram(options);
            this.Diagram.RegisterModelComponent<ConversationMessageModel, ConversationMessage>();

            var nodes = this.Response.Messages
                .Join(graphModel.Nodes, m => m.MessageId, n => n.UserData, (m,n) => new { m, n })
                .Select(x => new ConversationMessageModel(x.m, new Blazor.Diagrams.Core.Geometry.Point(x.n.Center.X, x.n.Center.Y))).ToList();
            var edges = this.Response.Messages.Where(n => n.Parent is not null).Select(
                m => new LinkModel(
                    nodes.First(n => n.Message.MessageId == m.MessageId),
                    nodes.FirstOrDefault(n => n.Message.MessageId == m.Parent.MessageId))
                {
                    TargetMarker = LinkMarker.Arrow,
                }).ToList();
            this.Diagram.Nodes.Add(nodes);
            this.Diagram.Links.Add(edges);

            this.StateHasChanged();
        }
    }
}
