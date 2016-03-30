using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.Models
{
    public class CommitAbstract
    {
        public CommitAbstract(
            string hash,
            Signature author,
            Signature committer,
            IList<string> parents,
            string message)
        {
            Hash = hash;
            ShortHash = hash.Substring(0, 7);
            Author = author;
            Committer = committer;

            Parents = new ReadOnlyCollection<string>(parents.ToList());
            Message = message;
        }

        public string Hash { get; }
        public string ShortHash { get; }
        public Signature Author { get; }
        public Signature Committer { get; }
        public DateTime Date { get; }
        public ReadOnlyCollection<string> Parents { get; }
        public string Message { get; }
    }
}
