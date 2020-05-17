using System;
using System.Runtime.Serialization;

namespace DecisionTree.Logic.Exceptions
{
    /// <summary>
    /// Represents the CsvRowFormatInvalidException class.
    /// </summary>
    [Serializable]
    public class CsvRowFormatInvalidException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvRowFormatInvalidException"/> class.
        /// </summary>
        public CsvRowFormatInvalidException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvRowFormatInvalidException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CsvRowFormatInvalidException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvRowFormatInvalidException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CsvRowFormatInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvRowFormatInvalidException"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context</param>
        protected CsvRowFormatInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}