using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EFPlayground.Data
{
    public static class TransactionFactory
    {
        public static TransactionScope Create()
        {
            return Create(IsolationLevel.Snapshot);
        }

        public static TransactionScope CreateAsync()
        {
            return CreateAsync(IsolationLevel.Snapshot);
        }

        public static TransactionScope Create(IsolationLevel isolationLevel)
        {
            var options = new TransactionOptions { IsolationLevel = isolationLevel };
            return new TransactionScope(TransactionScopeOption.Required, options);
        }

        public static TransactionScope CreateAsync(IsolationLevel isolationLevel)
        {
            var options = new TransactionOptions { IsolationLevel = isolationLevel };
            return new TransactionScope(TransactionScopeOption.Required, options, TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}
