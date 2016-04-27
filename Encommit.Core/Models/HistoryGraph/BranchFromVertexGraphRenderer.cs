using Encommit.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.Models.HistoryGraph
{
    public static class BranchFromVertexGraphRenderer
    {
        static public HistoryGraphItem<T> Render<T>(HistoryCommit<T> commit, IReadOnlyList<T> incomings)
        {
            var vertex = Vertex(commit, incomings);
            var inboundEdges = InboundEdges(commit, incomings);
            var outboundEdges = OutboundEdges(commit, incomings);
            var outgoings = Outgoings(commit, incomings);

            return new HistoryGraphItem<T>(vertex, inboundEdges, outboundEdges, outgoings);
        }

        static public HistoryVertex Vertex<T>(HistoryCommit<T> commit, IReadOnlyList<T> incomings)
        {
            return new HistoryVertex(incomings.IndexOf(commit.ID) ?? incomings.Count);
        }

        static public IReadOnlyList<HistoryEdge> InboundEdges<T>(HistoryCommit<T> commit, IReadOnlyList<T> incomings)
        {
            var comparer = EqualityComparer<T>.Default;

            var vertex = Vertex(commit, incomings);
            var result = new List<HistoryEdge>();
            int from = 0;
            int to = 0;
            foreach (var id in incomings)
            {
                result.Add(new HistoryEdge(from, comparer.Equals(id, commit.ID) ? vertex.Index : to));
                from++;
                to = (comparer.Equals(id, commit.ID) && vertex.Index < to) ? to : to + 1;
            }
            return result;
        }

        static public IReadOnlyList<T> Outgoings<T>(HistoryCommit<T> commit, IReadOnlyList<T> incomings)
        {
            var comparer = EqualityComparer<T>.Default;

            bool found = false;
            var result = new List<T>();
            foreach (var id in incomings)
            {
                if (comparer.Equals(id, commit.ID))
                {
                    if (!found)
                    {
                        result.Add(commit.Parents[0]);
                        found = true;
                    }
                }
                else
                {
                    result.Add(id);
                }
            }
            var newParents = incomings.Contains(commit.ID) ? commit.Parents.Skip(1) : commit.Parents;
            foreach (var id in newParents)
            {
                result.Add(id);
            }
            return result;
        }

        static public IReadOnlyList<HistoryEdge> OutboundEdges<T>(HistoryCommit<T> commit, IReadOnlyList<T> incomings)
        {
            var comparer = EqualityComparer<T>.Default;
            var result = new List<HistoryEdge>();

            bool found = false;
            var i = 0;
            foreach (var id in incomings)
            {
                if (comparer.Equals(commit.ID, id))
                {
                    if (commit.Parents.Count > 0 && !found)
                    {
                        result.Add(new HistoryEdge(i, i));
                        found = true;
                        i++;
                    }
                }
                else
                {
                    result.Add(new HistoryEdge(i, i));
                    i++;
                }
            }

            var vertex = Vertex(commit, incomings);
            var newParents = incomings.Contains(commit.ID) ? commit.Parents.Skip(1) : commit.Parents;
            foreach (var id in newParents)
            {
                result.Add(new HistoryEdge(vertex.Index, i));
                i++;
            }

            return result;
        }
    }
}
