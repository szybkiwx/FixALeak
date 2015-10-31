using System;
using System.Runtime.Serialization;

namespace FixALeak.Service
{
    [Serializable]
    internal class NavigationPropertyDoesNotExistException : Exception
    {
        public NavigationPropertyDoesNotExistException()
        {
        }

        public NavigationPropertyDoesNotExistException(string message) : base(message)
        {
        }

        public NavigationPropertyDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NavigationPropertyDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}