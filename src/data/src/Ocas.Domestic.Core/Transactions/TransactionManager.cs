using System;
using System.Threading.Tasks;

namespace Ocas.Domestic.Transactions
{
    public class TransactionManager : IDisposable
    {
        private bool _disposedValue;

        public Transaction Current { get; set; }

        public Task BeginTransaction()
        {
            if (Current != null) return Task.CompletedTask;

            Current = new Transaction();

            return Task.CompletedTask;
        }

        public Task Commit()
        {
            if (Current == null) return Task.CompletedTask;

            Current.Commit();
            Current = null;

            return Task.CompletedTask;
        }

        public Task Rollback()
        {
            if (Current == null) return Task.CompletedTask;

            Current.Rollback();
            Current = null;

            return Task.CompletedTask;
        }

        public Task Rollback(Exception exc)
        {
            if (Current == null) return Task.CompletedTask;

            Current.Rollback(exc);
            Current = null;

            return Task.CompletedTask;
        }

        #region IDisposable Support
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing)
            {
                // Don't leave an open transaction hanging around
                Current?.Rollback();
            }

            Current = null;

            _disposedValue = true;
        }
        #endregion IDisposable Support
    }
}
