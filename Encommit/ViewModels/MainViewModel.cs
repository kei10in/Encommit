﻿using Encommit.Models;
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
                .SelectMany(repository => repository.GetHistory())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(commit => History.Add(commit));
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

        private ObservableCollection<string> _history = new ObservableCollection<string>();
        public ObservableCollection<string> History
        {
            get { return _history; }
            set { this.RaiseAndSetIfChanged(ref _history, value); }
        }

        private void Load()
        {
            WorkingRespository = new GitRepository(_repositoryPath);
        }
    }
}
