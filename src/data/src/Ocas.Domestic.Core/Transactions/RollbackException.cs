using System;
using System.Runtime.Serialization;

namespace Ocas.Domestic.Transactions
{
    /// <summary>
    /// To be thrown when an enlisted resource manager does not support rollback.
    /// </summary>
    [Serializable]
    public class RollbackException : Exception
    {
        public RollbackException()
        {
        }

        public RollbackException(string message)
            : base(message)
        {
        }

        public RollbackException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RollbackException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
