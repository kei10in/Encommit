using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.Models.HistoryGraph
{
    public class HistoryVertex : IEquatable<HistoryVertex>
    {
        public HistoryVertex(int index)
        {
            Index = index;
        }

        public int Index { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as HistoryVertex);
        }

        public bool Equals(HistoryVertex rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode();
        }

        public static bool operator ==(HistoryVertex v1, HistoryVertex v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1.Index == v2.Index;
        }

        public static bool operator !=(HistoryVertex v1, HistoryVertex v2)
        {
            return !(v1 == v2);
        }
    }
}
