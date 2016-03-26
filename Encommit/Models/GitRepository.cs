using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.Models
{
    public class GitRepository
    {

        public GitRepository(string repositoryPath)
        {
            Path = repositoryPath;
        }

        public string Path { get; set; }

        public IObservable<string> GetHistory()
        {
            var r = new List<string> { "foo", "bar", "buz" };
            return r.ToObservable();
        }
    }
}
