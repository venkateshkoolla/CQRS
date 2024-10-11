using System;
using System.Collections.Generic;

namespace Ocas.Domestic.Transactions
{
    public class Transaction
    {
        private readonly List<IResourceManager> _resourceManagers = new List<IResourceManager>();

        public void Enlist(IResourceManager provider)
        {
            if (_resourceManagers.Contains(provider))
            {
                // Already enlisted
                return;
            }

            _resourceManagers.Add(provider);
        }

        public void Commit()
        {
            foreach (var rm in _resourceManagers)
            {
                rm.Commit();
            }
        }

        public void Rollback()
        {
            foreach (var rm in _resourceManagers)
            {
                rm.Rollback();
            }
        }

        public void Rollback(Exception exc)
        {
            foreach (var rm in _resourceManagers)
            {
                rm.Rollback(exc);
            }
        }
    }
}
