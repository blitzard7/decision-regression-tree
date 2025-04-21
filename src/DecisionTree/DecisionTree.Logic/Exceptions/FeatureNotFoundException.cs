using System;
using System.Runtime.Serialization;

namespace DecisionTree.Logic.Exceptions
{
    [Serializable]
    public class FeatureNotFoundException : Exception
    {
        public FeatureNotFoundException()
        {
        }

        public FeatureNotFoundException(string message) : base(message)
        {
        }

        public FeatureNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FeatureNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}