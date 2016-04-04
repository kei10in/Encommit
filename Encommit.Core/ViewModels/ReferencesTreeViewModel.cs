using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.ViewModels
{
    public class ReferencesTreeViewModel
    {
        public ReferenceGroupViewModel References { get; } = new ReferenceGroupViewModel("", null);

        public void Add(string branchName)
        {
            if (string.IsNullOrWhiteSpace(branchName))
            {
                throw new ArgumentException("Invalid branch name", nameof(branchName));
            }

            char[] delim = { '/' };
            var names = branchName.Split(delim);

            Add(names, References);
        }

        private void Add(IEnumerable<string> names, ReferenceGroupViewModel parent)
        {
            var name = names.First();
            var rest = names.Skip(1);

            var node = parent.Children.FirstOrDefault(x => x.Name == name);

            bool hasRest = rest.Any();
            bool hasNode = node != null;

            if (hasRest && hasNode)
            {
                var group = node as ReferenceGroupViewModel;
                if (group == null)
                {
                    throw new InvalidOperationException($"{node.FullName} is already exist.");
                }
                Add(rest, group);
            }
            else if (hasRest && !hasNode)
            {
                var group = new ReferenceGroupViewModel(name, parent);
                Add(rest, group);
                parent.Children.Add(group);
            }
            else if (!hasRest && hasNode)
            {
                throw new InvalidOperationException($"{node.FullName} is slready exist.");
            }
            else if (!hasRest && !hasNode)
            {
                var branch = new ReferenceViewModel(name, parent);
                parent.Children.Add(branch);
            }
        }
    }

    public interface IReferenceEntryViewModel
    {
        string FullName { get; }
        string Name { get; }
    }

    public class ReferenceEntryViewModel : IReferenceEntryViewModel
    {
        public ReferenceEntryViewModel(string name)
        {
            Name = name;
        }

        public ReferenceEntryViewModel(string name, ReferenceGroupViewModel parent)
        {
            Name = name;
            Parent = parent;
        }

        public string FullName
        {
            get
            {
                if (Parent == null)
                {
                    return Name;
                }
                else
                {
                    return string.IsNullOrWhiteSpace(Parent.FullName)
                        ? Name : Parent.FullName + "/" + Name;
                }
            }
        }

        public string Name { get; }

        protected ReferenceGroupViewModel Parent { get; }
    }

    public class ReferenceViewModel : ReferenceEntryViewModel
    {
        public ReferenceViewModel(string name)
            : base(name)
        { }

        public ReferenceViewModel(string name, ReferenceGroupViewModel parent)
            : base(name, parent)
        { }
    }

    public class ReferenceGroupViewModel : ReferenceEntryViewModel
    {
        public ReferenceGroupViewModel(string name)
            : base(name)
        { }

        public ReferenceGroupViewModel(string name, ReferenceGroupViewModel parent)
            : base(name, parent)
        { }

        public ObservableCollection<IReferenceEntryViewModel> Children { get; }
            = new ObservableCollection<IReferenceEntryViewModel>();
    }
}
