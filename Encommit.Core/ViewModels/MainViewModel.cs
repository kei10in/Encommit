using Encommit.Models;
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
    public class MainViewModel : ReactiveObject
    {
        public MainViewModel()
        {
            this.WhenAnyValue(x => x.RepositoryPath)
                .Where(path => path != null)
                .Subscribe(_ => Load());

            var repositoryLoaded = this.WhenAnyValue(x => x.WorkingRespository)
                .Where(repository => repository != null);

            repositoryLoaded
                .SelectMany(repository => repository.GetHistoryReactive())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(historyItem => History.Add(historyItem));

            repositoryLoaded
                .SelectMany(repository => repository.GetLocalBranchesReactive())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(branch => LocalBranches.Add(branch.Name));

            this.WhenAnyValue(x => x.SelectedHistoryItem)
                .Subscribe(selected => LoadCommitAbstract(selected));

            this.WhenAnyValue(x => x.SelectedHistoryItem)
                .Subscribe(selected => LoadTreeChanges(selected));

            this.WhenAnyValue(x => x.SelectedChange)
                .Subscribe(selected => LoadFilePatch(selected));
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

        private ObservableCollection<HistoryItem> _history = new ObservableCollection<HistoryItem>();
        public ObservableCollection<HistoryItem> History
        {
            get { return _history; }
            set { this.RaiseAndSetIfChanged(ref _history, value); }
        }

        private HistoryItem _selectedHistoryItem;
        public HistoryItem SelectedHistoryItem
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
            WorkingRespository.GetPatchReactive(SelectedHistoryItem.Id, filepath)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => FilePatch = new TextDocument(x.Content));
        }
    }
}
