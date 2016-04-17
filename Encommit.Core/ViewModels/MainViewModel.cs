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
            _tabs = new ReactiveList<TabItemViewModel>();

            AddTab = ReactiveCommand.Create();
            AddTab.ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => AddImpl());
        }

        private ReactiveList<TabItemViewModel> _tabs;
        public IReadOnlyReactiveCollection<TabItemViewModel> Tabs
        {
            get { return _tabs; }
        }

        public ReactiveCommand<object> AddTab { get; }

        void AddImpl()
        {
            var vm = new DashboardViewModel();
            var tab = new TabItemViewModel();
            tab.Content = vm;
            _tabs.Add(tab);

            vm.WhenAnyValue(x => x.Path)
                .Skip(1)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(path =>
                {
                    var rvm = new RepositoryViewModel();
                    rvm.RepositoryPath = path;
                    tab.Content = rvm;
                });
        }
    }
}
