using System;
using System.Runtime.Serialization;

namespace DecisionTree.Logic.Exceptions
{
    /// <summary>
    /// Represents the InvalidFileExtensionException class.
    /// </summary>
    [Serializable]
    public class InvalidFileExtensionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFileExtensionException"/> class.
        /// </summary>
        public InvalidFileExtensionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFileExtensionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidFileExtensionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFileExtensionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidFileExtensionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFileExtensionException"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context</param>
        protected InvalidFileExtensionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}