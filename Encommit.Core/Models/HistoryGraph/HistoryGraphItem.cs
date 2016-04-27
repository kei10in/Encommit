using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.Models.HistoryGraph
{
    public class HistoryGraphItem<T>
    {
        public HistoryGraphItem(
            HistoryVertex vertex,
            IEnumerable<HistoryEdge> inboundEdges,
            IEnumerable<HistoryEdge> outboundEdges,
            IEnumerable<T> outboundVertexes)
        {
            Vertex = vertex;
            InboundEdges = inboundEdges.ToArray();
            OutboundEdges = outboundEdges.ToArray();
            OutboundVertexes = outboundVertexes.ToArray();
        }

        public HistoryVertex Vertex { get; }
        public IReadOnlyList<HistoryEdge> InboundEdges { get; }
        public IReadOnlyList<HistoryEdge> OutboundEdges { get; }
        public IReadOnlyList<T> OutboundVertexes { get; }
    }
}
