using DecisionTree.Logic.Helper;
using DecisionTree.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Logic.Trees
{
    /* TODO:
     * Build Method where Tree is built recursivly.
    */
    public class Node : INode
    {
        public Node()
        {
            Children = new Dictionary<string, INode>();
        }

        public string Feature { get; set; }
        public INode Parent { get; set; }
        public Dictionary<string, INode> Children { get; set; }
        public bool IsLeaf => Children.Values.Count == 0;

        public void Build(CsvData data)
        {
            if (ContainsHomogeneousData(data))
            {
                return;
            }

            var feature = CalculateFeature(data);
            var featureValuesDistinct = feature.Values.Distinct();
            foreach (var distinctValue in featureValuesDistinct)
            {
                var node = new Node
                {
                    Parent = this,
                    Feature = feature.Name
                };
                var currentData = data.Filter(feature.Name, distinctValue);
                // If feature selected, the subset of csvdata should not contain the actual feature in header anymore.
                node.Build(currentData);
                this.Children.Add(distinctValue, node);
            }

            return;
        }

        public bool ContainsHomogeneousData(CsvData data)
        {
            // Check if rows contains only homogeneous data.
            // need to calculate the entropy 
            var distinctResultValues = data.GetUniqueColumnValues(data.ResultCategory).ToList();
            if (distinctResultValues.Count == 0)
            {
                throw new ArgumentException("Result values where empty.");
            }

            // data.headers.count == 1 -> represents the ResultCategory therefore we have homogeneous data.
            return distinctResultValues.Count == 1 || data.Headers.Count == 1;
        }

        private Feature CalculateFeature(CsvData data)
        {
            var informationGainOfFeatures = Calculation.CalculateInformationGainOfFeatures(data).ToList();

            // We have to calculate for each feature the IG and then select MAX(FeatureIG).
            var toSelectFeature = SelectFeatureForSplit(informationGainOfFeatures);
            var toSelectFeatureValues = data.Columns[toSelectFeature.Item1];
            var feature = new Feature()
            {
                Name = toSelectFeature.Item1,
                Values = toSelectFeatureValues
            };

            return feature;
        }

        private (string, double) SelectFeatureForSplit(List<(string, double)> informationGainOfFeatures)
        {
            // Select MAX(FeatureIG) of IG collection.
            // If n elements have the same highest IG, then select first occurence of it.
            var toSelectFeature = informationGainOfFeatures.Max(x => x.Item2);
            return informationGainOfFeatures.First(x => x.Item2 == toSelectFeature);
        }
    }
}
