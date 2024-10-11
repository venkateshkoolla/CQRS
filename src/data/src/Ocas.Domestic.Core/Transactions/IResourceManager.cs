using System;

namespace Ocas.Domestic.Transactions
{
    public interface IResourceManager
    {
        void Commit();
        void Rollback();
        void Rollback(Exception exc);
    }
}
