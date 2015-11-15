using System;
using System.Runtime.Serialization;

namespace FixALeak.JsonApiSerializer
{
    [Serializable]
    public class RelationshipUpdateForbiddenException : Exception
    {
        public RelationshipUpdateForbiddenException()
        {
        }

        public RelationshipUpdateForbiddenException(string message) : base(message)
        {
        }

        public RelationshipUpdateForbiddenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RelationshipUpdateForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}