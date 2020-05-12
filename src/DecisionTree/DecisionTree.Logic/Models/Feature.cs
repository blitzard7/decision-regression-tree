using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionTree.Logic.Models
{
    public class Feature
    {
        public string Name { get; set; }
        public double FeatureEntropy { get; set; }
        public double FeatureInformationGain { get; set; }
        public List<string> Values { get; set; }
    }
}
