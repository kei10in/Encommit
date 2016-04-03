using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Encommit.ViewModels
{
    public class ReactiveProperty<T> : ReactiveObject, IObservable<T>, IDisposable
    {
        bool disposed = false;
        private Subject<T> _subject = new Subject<T>();

        public ReactiveProperty()
        {
            this.WhenAnyValue(x => x.Value).Subscribe(_subject);
        }

        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                this.RaiseAndSetIfChanged(ref _value, value);
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _subject.Subscribe(observer);
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposed) return;

            disposed = true;

            if (disposing)
            {
                _subject?.Dispose();
                _subject = null;
            }
        }

        ~ReactiveProperty()
        {
            Dispose(false);
        }
    }
}
