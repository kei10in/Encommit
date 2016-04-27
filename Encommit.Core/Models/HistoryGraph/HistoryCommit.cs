using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.Models.HistoryGraph
{
    public class HistoryCommit<T>
    {
        public HistoryCommit(T id)
        {
            ID = id;
            Parents = Enumerable.Empty<T>().ToArray();
        }

        public HistoryCommit(T id, IEnumerable<T> parentes)
        {
            ID = id;
            Parents = parentes.ToArray();
        }

        public T ID { get; }

        public IReadOnlyList<T> Parents { get; }
    }
}
