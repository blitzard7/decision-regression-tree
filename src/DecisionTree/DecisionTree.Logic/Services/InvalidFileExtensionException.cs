using System;
using System.Runtime.Serialization;

namespace DecisionTree.Logic.Services
{
    [Serializable]
    internal class InvalidFileExtensionException : Exception
    {
        public InvalidFileExtensionException()
        {
        }

        public InvalidFileExtensionException(string message) : base(message)
        {
        }

        public InvalidFileExtensionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidFileExtensionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}