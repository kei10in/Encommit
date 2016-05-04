using Encommit.Models;
using Encommit.Models.HistoryGraph;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.ViewModels
{
    public class ShapeViewModel { }

    public sealed class VerticalLineViewModel : ShapeViewModel
    {
        public VerticalLineViewModel(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }

    public sealed class EllipseViewModel : ShapeViewModel
    {
        public EllipseViewModel(int index)
        {
            Index = index;
        }

        public int Index { get; }
    }

    public sealed class PathViewModel : ShapeViewModel
    {
        public PathViewModel(int from, int to, int y)
        {
            From = from;
            To = to;
            Y = y;
        }

        public int From { get; }
        public int To { get; }
        public int Y { get; }
    }

    public class HistoryItemViewModel : ReactiveObject
    {
        private HistoryItemViewModel() { }

        public HistoryItemViewModel(
            HistoryGraphItem<LibGit2Sharp.ObjectId> graph,
            HistoryItem commit)
        {
            Graph = graph;
            Commit = commit;

            _graphics = new List<ShapeViewModel>();
            foreach (var c in Graph.InboundEdges)
            {
                if (c.From == c.To)
                {
                    _graphics.Add(new VerticalLineViewModel(c.From, 0));
                }
                else
                {
                    _graphics.Add(new PathViewModel(c.From, c.To, 0));
                }
            }

            foreach (var c in Graph.OutboundEdges)
            {
                if (c.From == c.To)
                {
                    _graphics.Add(new VerticalLineViewModel(c.From, 1));
                }
                else
                {
                    _graphics.Add(new PathViewModel(c.From, c.To, 1));
                }
            }

            _graphics.Add(new EllipseViewModel(Graph.Vertex.Index));

            GraphWidth = Math.Max(Graph.InboundEdges.Count, Graph.OutboundEdges.Count);
        }

        private List<ShapeViewModel> _graphics;
        public IReadOnlyList<ShapeViewModel> Graphics { get { return _graphics; } }
        public int GraphWidth { get; }

        public HistoryGraphItem<LibGit2Sharp.ObjectId> Graph { get; }
        public HistoryItem Commit { get; }

        public HistoryItemViewModel Next(HistoryItem commit)
        {
            var history = new HistoryCommit<LibGit2Sharp.ObjectId>(commit.Id, commit.Parents);
            if (ReferenceEquals(this, Empty))
            {
                var graph = BranchFromVertexGraphRenderer.Render(history, new List<LibGit2Sharp.ObjectId>());
                return new HistoryItemViewModel(graph, commit);
            }
            else
            {
                var graph = BranchFromVertexGraphRenderer.Render(history, Graph.OutboundVertexes);
                return new HistoryItemViewModel(graph, commit);
            }
        }

        public static readonly HistoryItemViewModel Empty = new HistoryItemViewModel();
    }
}
