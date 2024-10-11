using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider : IDisposable, ICrmExtrasProvider
    {
        private readonly int _commandTimeout;
        private readonly string _connectionString;
        private bool _disposedValue;
        private IDbConnection _conn;

        // Register UTC DateTime handler and custom maps for Dapper
        static CrmExtrasProvider()
        {
            SqlMapper.AddTypeHandler(new DateTimeHandler());
        }

        public CrmExtrasProvider(string connectionString, int commandTimeout)
        {
            _connectionString = connectionString;
            _commandTimeout = commandTimeout;
        }

        protected IDbConnection Connection
        {
            get
            {
                if (_conn != null) return _conn;

                _conn = new SqlConnection(_connectionString);
                _conn.Open();

                return _conn;
            }
        }

        #region IDisposable Support
        // This code added to correctly implement the disposable pattern.
#pragma warning disable SA1202 // Elements must be ordered by access
        public void Dispose()
#pragma warning restore SA1202 // Elements must be ordered by access
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
                if (_conn?.State == ConnectionState.Open)
                {
                    try
                    {
                        _conn.Close();
                    }
                    catch
                    {
                        // ignore errors
                    }
                }

                _conn?.Dispose();
            }

            _conn = null;

            _disposedValue = true;
        }
        #endregion IDisposable Support
    }
}
