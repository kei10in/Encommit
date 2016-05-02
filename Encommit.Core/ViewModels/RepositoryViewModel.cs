using Encommit.Models;
using Encommit.Models.HistoryGraph;
using ICSharpCode.AvalonEdit.Document;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.ViewModels
{
    public class RepositoryViewModel : ReactiveObject, ITabContentViewModel
    {
        public RepositoryViewModel()
        {
            this.WhenAnyValue(x => x.RepositoryPath)
                .Where(path => path != null)
                .Subscribe(_ => Load());

            var repositoryLoaded = this.WhenAnyValue(x => x.WorkingRespository)
                .Where(repository => repository != null);

            repositoryLoaded
                .SelectMany(repository => repository.GetHistoryReactive())
                .Scan(
                    HistoryItemViewModel.Empty,
                    (HistoryItemViewModel vm, HistoryItem commit) =>
                    {
                        return vm.Next(commit);
                    })
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(graph => History.Add(graph));

            repositoryLoaded
                .SelectMany(repository => repository.GetLocalBranchesReactive())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(branch => LocalBranches.Add(branch.Name));

            repositoryLoaded
                .SelectMany(repository => repository.GetTagsReactive())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(tag => Tags.Add(tag.Name));

            repositoryLoaded
                .SelectMany(repository => repository.GetRemoteBranchesReactive())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(branch => Remotes.Add(branch));

            this.WhenAnyValue(x => x.SelectedHistoryItem)
                .Where(x => x != null)
                .Subscribe(selected => LoadCommitAbstract(selected.Commit));

            this.WhenAnyValue(x => x.SelectedHistoryItem)
                .Where(x => x != null)
                .Subscribe(selected => LoadTreeChanges(selected.Commit));

            this.WhenAnyValue(x => x.SelectedChange)
                .Subscribe(selected => LoadFilePatch(selected));
        }

        public string Header
        {
            get
            {
                if (RepositoryPath == null) return string.Empty;
                return RepositoryPath.Split().Last();
            }
        }

        private string _repositoryPath;
        public string RepositoryPath
        {
            get { return _repositoryPath; }
            set { this.RaiseAndSetIfChanged(ref _repositoryPath, value); }
        }

        private GitRepository _workingRepository;
        public GitRepository WorkingRespository
        {
            get { return _workingRepository; }
            set { this.RaiseAndSetIfChanged(ref _workingRepository, value); }
        }

        private ReferencesTreeViewModel _localBranches = new ReferencesTreeViewModel();
        public ReferencesTreeViewModel LocalBranches
        {
            get { return _localBranches; }
            set { this.RaiseAndSetIfChanged(ref _localBranches, value); }
        }

        private ReferencesTreeViewModel _tags = new ReferencesTreeViewModel();
        public ReferencesTreeViewModel Tags
        {
            get { return _tags; }
            set { this.RaiseAndSetIfChanged(ref _tags, value); }
        }

        public RemoteBranchViewModel Remotes { get; } = new RemoteBranchViewModel();

        private ObservableCollection<HistoryItemViewModel> _history = new ObservableCollection<HistoryItemViewModel>();
        public ObservableCollection<HistoryItemViewModel> History
        {
            get { return _history; }
            set { this.RaiseAndSetIfChanged(ref _history, value); }
        }

        private HistoryItemViewModel _selectedHistoryItem;
        public HistoryItemViewModel SelectedHistoryItem
        {
            get { return _selectedHistoryItem; }
            set { this.RaiseAndSetIfChanged(ref _selectedHistoryItem, value); }
        }

        private CommitAbstractViewModel _abstract;
        public CommitAbstractViewModel Abstract
        {
            get { return _abstract; }
            set { this.RaiseAndSetIfChanged(ref _abstract, value); }
        }

        private ObservableCollection<string> _changes;
        public ObservableCollection<string> Changes
        {
            get { return _changes; }
            set { this.RaiseAndSetIfChanged(ref _changes, value); }
        }

        private string _selectedChange;
        public string SelectedChange
        {
            get { return _selectedChange; }
            set { this.RaiseAndSetIfChanged(ref _selectedChange, value); }
        }

        private TextDocument _filePatch;
        public TextDocument FilePatch
        {
            get { return _filePatch; }
            set { this.RaiseAndSetIfChanged(ref _filePatch, value); }
        }

        private void Load()
        {
            WorkingRespository = new GitRepository(_repositoryPath);
        }

        private void LoadCommitAbstract(HistoryItem item)
        {
            if (item == null) return;
            WorkingRespository.GetCommitAbstractReactive(item.Id)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => Abstract = new CommitAbstractViewModel(x));
        }

        private void LoadTreeChanges(HistoryItem item)
        {
            if (item == null) return;
            Changes = new ObservableCollection<string>();
            WorkingRespository.GetTreeChangesReactive(item.Id)
                .ObserveOn(RxApp.MainThreadScheduler)
                .SelectMany(x => x)
                .Subscribe(x => Changes.Add(x.Path));
        }

        private void LoadFilePatch(string filepath)
        {
            if (filepath == null) return;
            WorkingRespository.GetPatchReactive(SelectedHistoryItem.Commit.Id, filepath)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => FilePatch = new TextDocument(x.Content));
        }
    }
}
