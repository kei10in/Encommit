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
            WorkingRepository = new RepositoryViewModel();
        }

        public RepositoryViewModel WorkingRepository { get; }
    }
}
