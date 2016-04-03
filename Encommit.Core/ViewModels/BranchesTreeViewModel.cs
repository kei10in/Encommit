using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.ViewModels
{
    public class BranchesTreeViewModel
    {
        public BranchGroupViewModel Branches { get; } = new BranchGroupViewModel("", null);

        public void Add(string branchName)
        {
            if (string.IsNullOrWhiteSpace(branchName))
            {
                throw new ArgumentException("Invalid branch name", nameof(branchName));
            }

            char[] delim = { '/' };
            var names = branchName.Split(delim);

            Add(names, Branches);
        }

        private void Add(IEnumerable<string> names, BranchGroupViewModel parent)
        {
            var name = names.First();
            var rest = names.Skip(1);

            var node = parent.Children.FirstOrDefault(x => x.Name == name);

            bool hasRest = rest.Any();
            bool hasNode = node != null;

            if (hasRest && hasNode)
            {
                var group = node as BranchGroupViewModel;
                if (group == null)
                {
                    throw new InvalidOperationException($"{node.FullName} is already exist.");
                }
                Add(rest, group);
            }
            else if (hasRest && !hasNode)
            {
                var group = new BranchGroupViewModel(name, parent);
                Add(rest, group);
                parent.Children.Add(group);
            }
            else if (!hasRest && hasNode)
            {
                throw new InvalidOperationException($"{node.FullName} is slready exist.");
            }
            else if (!hasRest && !hasNode)
            {
                var branch = new BranchViewModel(name, parent);
                parent.Children.Add(branch);
            }
        }
    }

    public interface IBranchEntryViewModel
    {
        string FullName { get; }
        string Name { get; }
    }

    public class BranchEntryViewModel : IBranchEntryViewModel
    {
        public BranchEntryViewModel(string name)
        {
            Name = name;
        }

        public BranchEntryViewModel(string name, BranchGroupViewModel parent)
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

        protected BranchGroupViewModel Parent { get; }
    }

    public class BranchViewModel : BranchEntryViewModel
    {
        public BranchViewModel(string name)
            : base(name)
        { }

        public BranchViewModel(string name, BranchGroupViewModel parent)
            : base(name, parent)
        { }
    }

    public class BranchGroupViewModel : BranchEntryViewModel
    {
        public BranchGroupViewModel(string name)
            : base(name)
        { }

        public BranchGroupViewModel(string name, BranchGroupViewModel parent)
            : base(name, parent)
        { }

        public ObservableCollection<IBranchEntryViewModel> Children { get; }
            = new ObservableCollection<IBranchEntryViewModel>();
    }
}
