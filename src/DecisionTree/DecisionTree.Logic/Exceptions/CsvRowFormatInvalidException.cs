using System;
using System.Runtime.Serialization;

namespace DecisionTree.Logic.Exceptions
{
    [Serializable]
    public class CsvRowFormatInvalidException : Exception
    {
        public CsvRowFormatInvalidException()
        {
        }

        public CsvRowFormatInvalidException(string message) : base(message)
        {
        }

        public CsvRowFormatInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CsvRowFormatInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}