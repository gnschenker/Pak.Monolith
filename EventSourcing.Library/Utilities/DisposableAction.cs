using System;
using System.Threading;

namespace EventSourcing.Library
{
    internal sealed class DisposableAction : IDisposable
    {

        public static readonly DisposableAction Empty = new DisposableAction(null);

        private Action _disposeAction;

        public DisposableAction(Action disposeAction)
        {
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            // Interlocked allows the continuation to be executed only once
            Action continuation = Interlocked.Exchange(ref _disposeAction, null);
            if (continuation != null)
            {
                continuation();
            }
        }

    }
}
