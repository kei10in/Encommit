using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
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

        public IObservable<HistoryItem> GetHistoryReactive()
        {
            return Observable.Create<HistoryItem>(observer =>
            {
                using (var repo = new Repository(Path))
                {
                    foreach (var commit in repo.Commits.Take(10))
                    {
                        observer.OnNext(new HistoryItem(commit.Id, commit.MessageShort));
                    }
                }
                observer.OnCompleted();

                return Disposable.Empty;
            });
        }

        public IObservable<CommitAbstract> GetCommitAbstractReactive(ObjectId id)
        {
            return Observable.Create<CommitAbstract>(observer =>
            {
                using (var repo = new Repository(Path))
                {
                    GitObject obj = repo.Lookup(id);
                    Commit commit = obj as Commit;
                    if (commit != null)
                    {
                        observer.OnNext(
                            new CommitAbstract(
                                commit.Sha,
                                commit.Author,
                                commit.Committer,
                                commit.Parents.Select(x => x.Sha).ToList(),
                                commit.Message));
                    }
                }
                observer.OnCompleted();

                return Disposable.Empty;
            });
        }
    }
}
