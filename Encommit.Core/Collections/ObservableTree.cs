using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.Collections
{
    public class ObservableTree<T>
    {
        private ObservableCollection<ObservableTree<T>> _children
            = new ObservableCollection<ObservableTree<T>>();

        public ObservableTree() : this(default(T))
        { }

        public ObservableTree(T value)
        {
            Value = value;
            Children = new ReadOnlyObservableCollection<ObservableTree<T>>(_children);
        }

        public ObservableTree<T> Parent { get; private set; }

        public ReadOnlyObservableCollection<ObservableTree<T>> Children { get; }

        public T Value { get; set; }

        public void Add(T value)
        {
            var child = new ObservableTree<T>() { Value = value };

            _children.Add(child);
            child.Parent = this;
        }

        public void Add(ObservableTree<T> child)
        {
            var c = child.Parent == null ? child : child.Duplicate();
            _children.Add(c);
            c.Parent = this;
        }

        public bool Remove(T value)
        {
            var comparer = EqualityComparer<T>.Default;

            var node = _children.FirstOrDefault(x => comparer.Equals(x.Value, value));
            if (node == null) return false;

            _children.Remove(node);
            return true;
        }

        private ObservableTree<T> Duplicate()
        {
            var result = new ObservableTree<T>(Value);

            foreach (var child in Children)
            {
                var newChild = child.Duplicate();
                result.Add(newChild);
            }

            return result;
        }
    }
}
