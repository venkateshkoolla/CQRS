using System;
using System.Threading.Tasks;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext : IDisposable
    {
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
        Task RollbackTransaction(Exception exc);
    }
}
