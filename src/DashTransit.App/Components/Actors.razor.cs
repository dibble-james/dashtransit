// <copyright file="Actors.razor.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.App.Components;

using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Core.Application.Queries;
using Core.Domain;

public partial class Actors
{
    private async Task<Diagram?> LoadActors()
    {
        var actors = await this.Mediator.Send(new MessageActors(this.MessageId));
        var diagram = new Diagram();

        var initiator = actors.First(x => x is Sender or Publisher);
        var initiatorNode = new NodeModel { Title = initiator.Endpoint.ToString() };
        initiatorNode.AddPort(PortAlignment.Right);
        diagram.Nodes.Add(initiatorNode);

        foreach (var consumer in actors.Where(x => x is Consumer))
        {
            var node = new NodeModel { Title = consumer.Endpoint.ToString() };
            node.AddPort(PortAlignment.Left);

            diagram.Nodes.Add(node);
            diagram.Links.Add(new LinkModel(initiatorNode.GetPort(PortAlignment.Right), node.GetPort(PortAlignment.Left)));
        }

        diagram.AutoArrange();

        return diagram;
    }
}