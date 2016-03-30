using Encommit.Models;
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

            this.WhenAnyValue(x => x.WorkingRespository)
                .Where(repository => repository != null)
                .SelectMany(repository => repository.GetHistoryReactive())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(historyItem => History.Add(historyItem));

            this.WhenAnyValue(x => x.SelectedHistoryItem)
                .Subscribe(selected => LoadCommitAbstract(selected));
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
    }
}
