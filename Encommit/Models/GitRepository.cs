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
            return WithCommit<CommitAbstract>(id, (repo, commit, observer) =>
            {
                observer.OnNext(
                    new CommitAbstract(
                        commit.Sha,
                        commit.Author,
                        commit.Committer,
                        commit.Parents.Select(x => x.Sha).ToList(),
                        commit.Message));
                observer.OnCompleted();

                return Disposable.Empty;
            });
        }

        public IObservable<TreeChanges> GetTreeChangesReactive(ObjectId id)
        {
            return WithCommit<TreeChanges>(id, (repo, commit, observer) =>
            {
                var oldTree = commit.Parents.FirstOrDefault()?.Tree;
                var newTree = commit.Tree;
                var changes = repo.Diff.Compare<TreeChanges>(oldTree, newTree);
                observer.OnNext(repo.Diff.Compare<TreeChanges>(oldTree, newTree));
                observer.OnCompleted();

                return Disposable.Empty;
            });
        }

        private IObservable<T> WithCommit<T>(
            ObjectId id, Func<Repository, Commit, IObserver<T>, IDisposable> subscribe)
        {
            return Observable.Create<T>(observer =>
            {
                using (var repo = new Repository(Path))
                {
                    GitObject obj = repo.Lookup(id);
                    Commit commit = obj as Commit;
                    if (commit != null)
                    {
                        return subscribe(repo, commit, observer);
                    }
                }
                return Disposable.Empty;
            });
        }
    }
}
