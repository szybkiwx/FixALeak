using System;
using System.Runtime.Serialization;

namespace FixALeak.JsonApiSerializer
{
    [Serializable]
    public class MalformedJsonApiDocumentException : Exception
    {
        public MalformedJsonApiDocumentException()
        {
        }

        public MalformedJsonApiDocumentException(string message) : base(message)
        {
        }

        public MalformedJsonApiDocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MalformedJsonApiDocumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}