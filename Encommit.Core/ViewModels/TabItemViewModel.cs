using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.ViewModels
{
    public class TabItemViewModel : ReactiveObject
    {
        private ITabContentViewModel _content;
        public ITabContentViewModel Content
        {
            get { return _content; }
            set { this.RaiseAndSetIfChanged(ref _content, value); }
        }

        public string Header
        {
            get
            {
                if (Content == null) return string.Empty;
                return Content.Header;
            }
        }
    }
}
