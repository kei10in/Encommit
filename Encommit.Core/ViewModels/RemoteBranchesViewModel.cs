using Encommit.Collections;
using Encommit.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.ViewModels
{
    public interface IRemoteBranch
    {
        string DisplayName { get; }
        string Name { get; }
    }

    public class RemoteRoot : IRemoteBranch
    {
        public RemoteRoot(string name)
        {
            DisplayName = name;
            Name = name;
        }

        public string DisplayName { get; }

        public string Name { get; }

    }

    public class RemoteBranchLeaf : IRemoteBranch
    {
        public RemoteBranchLeaf(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        public string DisplayName { get; }

        public string Name { get; }
    }

    public class RemoteBranchGroup : IRemoteBranch
    {
        public RemoteBranchGroup(string name)
        {
            DisplayName = name;
            Name = name;
        }

        public string DisplayName { get; }

        public string Name { get; }
    }


    public class RemoteBranchViewModel
    {
        public RemoteBranchViewModel()
        {
            Roots = new ObservableTree<IRemoteBranch>();
        }

        public ObservableTree<IRemoteBranch> Roots { get; }

        public void Add(RemoteBranch branch)
        {
            var remote = Roots.Children.FirstOrDefault(x => x.Value.Name == branch.Remote);
            if (remote == null)
            {
                remote = new ObservableTree<IRemoteBranch>(new RemoteRoot(branch.Remote));
                Roots.Add(remote);
            }

            char[] delim = { '/' };

            var branchPath = branch.Name.Replace(branch.Remote + "/", "").Split(delim).ToList();
            var branchBasePath = branchPath.Take(branchPath.Count() - 1);
            var branchLastName = branchPath.Last();

            var parent = remote.FindOrCreate(branchBasePath);
            if (parent == null)
            {
                parent = remote;
            }

            parent.Add(new RemoteBranchLeaf(branch.Name, branchLastName));
        }
    }


    public static class ObservableTreeExtensions
    {
        public static ObservableTree<IRemoteBranch> FindOrCreate(
            this ObservableTree<IRemoteBranch> self, IEnumerable<string> path)
        {
            if (path == null || !path.Any())
            {
                return self;
            }
            else
            {
                var name = path.First();
                var rest = path.Skip(1);

                var node = self.Children.FirstOrDefault(x => x.Value.Name == name);
                if (node == null)
                {
                    var result = new ObservableTree<IRemoteBranch>(new RemoteBranchGroup(name));
                    self.Add(result);
                    node = result;
                }

                return node.FindOrCreate(rest);
            }
        }
    }
}
