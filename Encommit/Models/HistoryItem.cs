using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.Models
{
    public class HistoryItem
    {
        public HistoryItem(ObjectId id, string messageShort)
        {
            Id = id;
            MessageShort = messageShort;
        }

        public ObjectId Id { get; }

        public string MessageShort { get; }
    }
}
