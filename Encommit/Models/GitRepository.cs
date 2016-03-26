using LibGit2Sharp;
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

        public IObservable<string> GetHistoryReactive()
        {
            return Observable.Create<string>(observer =>
            {
                using (var repo = new Repository(Path))
                {
                    foreach (var commit in repo.Commits.Take(10))
                    {
                        observer.OnNext(commit.MessageShort);
                    }
                }
                observer.OnCompleted();

                return () => { };
            });
        }
    }
}
