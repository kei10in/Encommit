using Encommit.Collections;
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
        public ObservableTree<IReferenceEntryViewModel> References { get; }
            = new ObservableTree<IReferenceEntryViewModel>();

        public void Add(string branchName)
        {
            if (string.IsNullOrWhiteSpace(branchName))
            {
                throw new ArgumentException("Invalid branch name", nameof(branchName));
            }

            char[] delim = { '/' };
            var names = branchName.Split(delim);

            Add(names, References, branchName);
        }

        private void Add(
            IEnumerable<string> names,
            ObservableTree<IReferenceEntryViewModel> parent,
            string branchName
            )
        {
            var name = names.First();
            var rest = names.Skip(1);

            var node = parent.Children.FirstOrDefault(x => x.Value.Name == name);
            bool hasRest = rest.Any();

            if (node == null && hasRest)
            {
                var group = new ReferenceGroupViewModel(name);
                node = new ObservableTree<IReferenceEntryViewModel>(group);
                parent.Add(node);
            }

            if (node == null)
            {
                parent.Add(new ReferenceViewModel(branchName));
            }
            else if (node.Value is ReferenceGroupViewModel && hasRest)
            {
                Add(rest, node, branchName);
            }
            else
            {
                throw new InvalidOperationException($"{branchName} is already exist.");
            }
        }
    }

    public interface IReferenceEntryViewModel
    {
        string Name { get; }
    }

    public class ReferenceViewModel : IReferenceEntryViewModel
    {
        public ReferenceViewModel(string name)
        {
            FullName = name;

            char[] delim = { '/' };
            var names = name.Split(delim);
            Name = names.LastOrDefault();
        }

        public string FullName { get; }

        public string Name { get; }
    }

    public class ReferenceGroupViewModel : IReferenceEntryViewModel
    {
        public ReferenceGroupViewModel(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
