using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Crm.Extras.Provider;
using Ocas.Domestic.Crm.Provider;
using Ocas.Domestic.Data.Mappers;
using Ocas.Domestic.Transactions;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext : IDomesticContext
    {
        private readonly IDomesticContextConfig _config;
        private readonly ILogger _logger;
        private TransactionManager _transactionManager = new TransactionManager();
        private CrmProvider _crmProvider;
        private CrmExtrasProvider _crmExtrasProvider;
        private bool _disposedValue;

        internal static CrmMapper CrmMapper { get; } = new CrmMapper();

        public DomesticContext(IDomesticContextConfig config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public Task BeginTransaction()
        {
            return _transactionManager.BeginTransaction();
        }

        public Task CommitTransaction()
        {
            return _transactionManager.Commit();
        }

        public Task RollbackTransaction()
        {
            return _transactionManager.Rollback();
        }

        public Task RollbackTransaction(Exception exc)
        {
            return _transactionManager.Rollback(exc);
        }

        protected CrmProvider CrmProvider
        {
            get
            {
                if (_crmProvider != null) return _crmProvider;

                _crmProvider = new CrmProvider(_config.CrmConnectionString, _config.CrmWcfServiceUrl, _logger);
                _crmProvider.SetTransactionManager(_transactionManager);

                return _crmProvider;
            }
        }

        protected ICrmExtrasProvider CrmExtrasProvider
        {
            get
            {
                if (_crmExtrasProvider != null) return _crmExtrasProvider;

                _crmExtrasProvider = new CrmExtrasProvider(_config.CrmExtrasConnectionString, _config.CommandTimeout);

                return _crmExtrasProvider;
            }
        }

        #region IDisposable Support
        // This code added to correctly implement the disposable pattern.
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
                _transactionManager?.Dispose();
                _crmProvider?.Dispose();
                _crmExtrasProvider?.Dispose();
            }

            _transactionManager = null;
            _crmProvider = null;
            _crmExtrasProvider = null;

            _disposedValue = true;
        }
        #endregion IDisposable Support
    }
}
