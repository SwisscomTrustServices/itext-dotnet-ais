using System;

namespace AIS.Common
{
    public class AisClientException : ApplicationException
    {
        public AisClientException(string message) : base(message) { }
        public AisClientException(string message, Exception e) : base(message, e) { }
    }
}
