using System.Collections.Generic;

namespace DecisionTree.Logic.Models
{
    /// <summary>
    /// Represents the Feature class.
    /// </summary>
    public class Feature
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        public double FeatureEntropy { get; set; }
        public double FeatureInformationGain { get; set; }
        public List<string> Values { get; set; }
    }
}
