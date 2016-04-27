using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.Models.HistoryGraph
{
    public class HistoryEdge : IEquatable<HistoryEdge>
    {
        public HistoryEdge(int from, int to)
        {
            From = from;
            To = to;
        }

        public int From { get; }
        public int To { get; }

        public override string ToString()
        {
            return $"From: {From}, To: {To}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as HistoryEdge);
        }

        public bool Equals(HistoryEdge rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return From.GetHashCode() ^ To.GetHashCode();
        }

        public static bool operator ==(HistoryEdge v1, HistoryEdge v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1.From == v2.From && v1.To == v2.To;
        }

        public static bool operator !=(HistoryEdge v1, HistoryEdge v2)
        {
            return !(v1 == v2);
        }
    }
}
