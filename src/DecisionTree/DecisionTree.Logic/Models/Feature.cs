using System.Collections.Generic;

namespace DecisionTree.Logic.Models
{
    /// <summary>
    /// Represents the Feature class.
    /// </summary>
    public class Feature
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the entropy.
        /// </summary>
        public double FeatureEntropy { get; set; }

        /// <summary>
        /// Gets or sets the information gain.
        /// </summary>
        public double FeatureInformationGain { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        public List<string> Values { get; set; }
    }
}
