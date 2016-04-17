using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Encommit.ViewModels
{
    public class DashboardViewModel : ReactiveObject, ITabContentViewModel
    {
        public string Header
        {
            get { return "Dashboard"; }
        }

        private string _path = string.Empty;
        public string Path
        {
            get { return _path; }
            set { this.RaiseAndSetIfChanged(ref _path, value); }
        }
    }
}
