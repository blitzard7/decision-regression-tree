using System;
using System.Runtime.Serialization;

namespace DecisionTree.Logic.Exceptions
{
    /// <summary>
    /// Represents the InvalidFileExtensionException class.
    /// </summary>
    [Serializable]
    public class FeatureNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureNotFoundException"/> class.
        /// </summary>
        public FeatureNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public FeatureNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public FeatureNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context</param>
        protected FeatureNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}