using System.Data;

namespace ChatApp.Bus.Transaction
{
    public class TransactionManager : IDisposable
    {
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;

        public TransactionManager(IDbConnection connection)
        {
            _connection = connection;
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction?.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction = null;
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                Rollback();
            }
            _connection?.Close();
            _connection = null;
        }
    }

}
