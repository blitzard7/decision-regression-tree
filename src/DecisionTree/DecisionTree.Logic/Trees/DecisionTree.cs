using System;
using System.Collections.Generic;
using System.Linq;
using DecisionTree.Logic.Exceptions;
using DecisionTree.Logic.Interfaces;
using DecisionTree.Logic.Models;

namespace DecisionTree.Logic.Trees
{
    /* TODO:
     * Query Tree and return path
     * Node should be adapted to contain the resultValue
     */

    /// <summary>
    /// Represents the DecisionTree class.
    /// </summary>
    public class DecisionTree : IDecisionTree
    {
        /// <summary>
        /// Gets or sets the root node.
        /// </summary>
        public INode Root { get; set; }

        // Build tree from given CsvData
        // Recursive Task?
        // Entropy and InformationGain is calculated 
        // Stop groving after all leafe nodes are homogeneouse (Post-pruning)
        // Returns DecisionTree

        /// <summary>
        /// Builds the tree according to the <see cref="CsvData"/> recursively.
        /// </summary>
        /// <param name="data">The csv data.</param>
        /// <returns>Returns the constructed tree.</returns>
        public ITree BuildTree(CsvData data)
        {
            // 1. determine root node (column) of tree
            // 2. calculate for each search key of current node (first node is root) the entropy and IG for next node (child)
            // 3. repeat 2 until no split is needed (meaning that all leafe nodes contain homogeneous data)

            // Assumption: Last column represents query result.
            // For E(G) we have to extract all result values 
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
                var mySearchThingy = tempNode.Feature;
                
                if (!searchKeys.TryGetValue(mySearchThingy, out var myChildKeyThingy))
                {
                    throw new InvalidOperationException($"No value for feature {mySearchThingy} have been provided.");
                }

                if (!tempNode.Children.TryGetValue(myChildKeyThingy, out var tmpChildNode))
                {
                    throw new FeatureNotFoundException($"Getting child node by querying tree with key {myChildKeyThingy} did not result in any node.");
                }

                tempNode = tmpChildNode;
            }

            return tempNode;
        }
    }
}
