using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Ocas.Domestic.Coltrane.Bds.Provider
{
    public partial class ColtraneBdsProvider : IDisposable, IColtraneBdsProvider
    {
        private readonly string _connectionString;
        private readonly int _commandTimeout;
        private IDbConnection _conn;

        // Register UTC DateTime handler and custom maps for Dapper
        static ColtraneBdsProvider()
        {
            SqlMapper.AddTypeHandler(new DateTimeHandler());
        }

        public ColtraneBdsProvider(string connectionString, int commandTimeout)
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
        private bool _disposedValue; // To detect redundant calls

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
                _conn?.Dispose();
            }

            _conn = null;

            _disposedValue = true;
        }
        #endregion IDisposable Support
    }
}
