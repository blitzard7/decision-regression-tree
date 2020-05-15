using System;
using System.Runtime.Serialization;

namespace DecisionTree.Logic.Exceptions
{
    /// <summary>
    /// Represents the CsvDataInvalidMetadataException class.
    /// </summary>
    [Serializable]
    public class CsvDataInvalidMetadataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataInvalidMetadataException"/> class.
        /// </summary>
        public CsvDataInvalidMetadataException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataInvalidMetadataException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CsvDataInvalidMetadataException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataInvalidMetadataException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CsvDataInvalidMetadataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataInvalidMetadataException"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context</param>
        protected CsvDataInvalidMetadataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}