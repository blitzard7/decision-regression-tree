using System;
using System.Collections.Generic;
using DecisionTree.Logic.Exceptions;
using DecisionTree.Logic.Interfaces;
using DecisionTree.Logic.Models;

namespace DecisionTree.Logic.Trees
{
    /// <summary>
    /// Represents the DecisionTree class.
    /// </summary>
    public class DecisionTree : IDecisionTree
    {
        /// <summary>
        /// Gets or sets the root node.
        /// </summary>
        public INode Root { get; set; }

        /// <summary>
        /// Builds the tree according to the <see cref="CsvData"/> recursively.
        /// </summary>
        /// <param name="data">The csv data.</param>
        /// <returns>Returns the constructed tree.</returns>
        public ITree BuildTree(CsvData data)
        {
            Root = new Node();
            Root.Start(data);
            return this;
        }

        /// <summary>
        /// Queries the constructed tree with the given search keys.
        /// </summary>
        /// <param name="searchKeys">The search keys.</param>
        /// <exception cref="FeatureNotFoundException">Is thrown when feature is not found.</exception>
        /// <returns>Returns the resulted node.</returns>
        public INode Query(Dictionary<string, string> searchKeys)
        {
            if (Root == null)
            {
                return null;
            }

            var tempNode = Root;

            while (!tempNode.IsLeaf)
            {
                var searchFeature = tempNode.Feature;
                
                if (!searchKeys.TryGetValue(searchFeature, out var searchFeatureValue))
                {
                    throw new InvalidOperationException($"No value for feature {searchFeature} have been provided.");
                }

                if (!tempNode.Children.TryGetValue(searchFeatureValue, out var tmpChildNode))
                {
                    throw new FeatureNotFoundException($"Getting child node by querying tree with key {searchFeatureValue} did not result in any node.");
                }

                tempNode = tmpChildNode;
            }

            return tempNode;
        }
    }
}
