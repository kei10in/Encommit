using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public string RepositoryPath
        {
            get; set;
        }
    }
}
