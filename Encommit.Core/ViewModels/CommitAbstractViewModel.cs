using Encommit.Models;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Encommit.ViewModels
{
    public class CommitAbstractViewModel
    {
        private CommitAbstract _overview;

        public CommitAbstractViewModel(CommitAbstract overview)
        {
            _overview = overview;
            Commit = $"{overview.Hash} [{overview.ShortHash}]";
            Author = FormatSignature(overview.Author);
            Committer = FormatSignature(overview.Committer);
            Parents = string.Join(", ", overview.Parents.Select(x => "[" + x.Substring(0, 7) + "]"));
            Message = overview.Message;
        }

        private string FormatSignature(Signature signature)
        {
            return signature.Name + " at " + signature.When.ToString(CultureInfo.CurrentUICulture.DateTimeFormat);
        }

        public string Commit { get; }
        public string Author { get; }
        public string Committer { get; }
        public string Parents { get; }
        public string Message { get; }

    }
}
