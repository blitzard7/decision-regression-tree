using System;
using System.Runtime.Serialization;

namespace DecisionTree.Logic.Exceptions
{
    [Serializable]
    public class CsvDataInvalidMetadataException : Exception
    {
        public CsvDataInvalidMetadataException()
        {
        }

        public CsvDataInvalidMetadataException(string message) : base(message)
        {
        }
        public CsvDataInvalidMetadataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CsvDataInvalidMetadataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}