using DecisionTree.Logic.Models;
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

        public string SearchString { get; set; }
        public INode Parent { get; set; }
        public Dictionary<string, INode> Children { get; set; }
        public bool IsLeaf => Children.Values.Count == 0;

        public INode Build(CsvData data)
        {
            if (ContainsHomogeneousData(this))
            {
                return this;
            }

            // feature = CalculateFeature()
            /* for (distinctValue in feature)
             *      node = new Node()
             *      node.Build(c.Fitter(distinctValue))
             *      this.Children.Append(node)
             */

            return this;
        }

        public bool ContainsHomogeneousData(INode node)
        {
            throw new System.NotImplementedException();
        }

        private void CalculateFeature()
        {
            //var informationGainOfFeatures = CalculateInformationGainOfFeatures(data, eg).ToList();

            // We have to calculate for each feature the IG and then select MAX(FeatureIG).
            //var toSelectFeature = SelectFeatureForSplit(informationGainOfFeatures);
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
