// <copyright file="GraphExtensions.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace Blazor.Diagrams.Core;

using Blazor.Diagrams.Core.Models;
using GraphShape.Algorithms.Layout;
using QuikGraph;

public static class GraphExtensions
{
    public static Diagram AutoArrange(this Diagram diagram)
    {
        var graph = new BidirectionalGraph<NodeModel, Edge<NodeModel>>();
        var nodes = diagram.Nodes.OfType<NodeModel>().ToList();
        var edges = diagram.Links.OfType<LinkModel>()
            .Select(lm =>
            {
                var source = nodes.Single(dn => dn.Id == lm.SourceNode.Id);
                var target = nodes.Single(dn => dn.Id == lm?.TargetNode?.Id);
                return new Edge<NodeModel>(source, target);
            })
            .ToList();
        graph.AddVertexRange(nodes);
        graph.AddEdgeRange(edges);

        // run GraphShape algorithm
        var positions = nodes.ToDictionary(nm => nm, dn => new GraphShape.Point(dn.Position.X, dn.Position.Y));
        var sizes = nodes.ToDictionary(nm => nm, dn => new GraphShape.Size(dn.Size?.Width ?? 100, dn.Size?.Height ?? 100));
        var layoutCtx = new LayoutContext<NodeModel, Edge<NodeModel>, BidirectionalGraph<NodeModel, Edge<NodeModel>>>(graph, positions, sizes, LayoutMode.Simple);
        var algoFact = new StandardLayoutAlgorithmFactory<NodeModel, Edge<NodeModel>, BidirectionalGraph<NodeModel, Edge<NodeModel>>>();
        var algo = algoFact.CreateAlgorithm("Tree", layoutCtx, new SimpleTreeLayoutParameters { Direction = LayoutDirection.LeftToRight, VertexGap = 100, LayerGap = 100 });

        algo.Compute();

        // update NodeModel positions
        try
        {
            diagram.SuspendRefresh = true;
            foreach (var vertPos in algo.VerticesPositions)
            {
                // NOTE;  have to use SetPosition which takes care of updating everything
                vertPos.Key.SetPosition(vertPos.Value.X, vertPos.Value.Y);
            }
        }
        finally
        {
            diagram.SuspendRefresh = false;
        }

        return diagram;
    }
}