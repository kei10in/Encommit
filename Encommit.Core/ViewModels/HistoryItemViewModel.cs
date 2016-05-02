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
    public class HistoryItemViewModel : ReactiveObject
    {
        private HistoryItemViewModel() { }

        public HistoryItemViewModel(
            HistoryGraphItem<LibGit2Sharp.ObjectId> graph,
            HistoryItem commit)
        {
            Graph = graph;
            Commit = commit;
        }

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
