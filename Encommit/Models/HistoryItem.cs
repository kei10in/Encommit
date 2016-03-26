using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.Models
{
    public class HistoryItem
    {
        public HistoryItem(string messageShort)
        {
            MessageShort = messageShort;
        }

        public string MessageShort { get; }
    }
}
