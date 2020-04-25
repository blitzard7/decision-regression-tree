using System;
using System.Runtime.Serialization;

namespace DecisionTree.Logic.Services
{
    [Serializable]
    public class CsvInvalidException : Exception
    {
        public CsvInvalidException()
        {
        }

        public CsvInvalidException(string message) : base(message)
        {
        }

        public CsvInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CsvInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}