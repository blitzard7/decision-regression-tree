using System.Collections.Generic;
using System.Linq;
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
        /// <returns>Returns the resulted node.</returns>
        public INode Query(List<(string featureName, string featureValue)> searchKeys)
        {
            // List of searchkey should contain rootfeature name as first element.
            // If there is no tree then return null.
            if (Root == null)
            {
                return null;
            }

            var tmpRoot = Root;

            // Assumption Rootnode is on index 0.
            var (featureName, featureValue) = searchKeys[0];
            var foundNode = QueryNode((featureName, featureValue));
            var foundNodeAllLeaves = foundNode.Children.All(x => x.Value.IsLeaf);

            if (foundNodeAllLeaves)
            {
                return foundNode;
            }

            if (foundNode.Children.Count != 0)
            {
                // If the query elements contains all column names, but the decisiontree did not split at all features 
                // we have to jump over some searchkeys if no split there is given
                // e.g. (Overcast, sunny), (temperature, hot) -> temperature is no split category so we need to jump this!
                for (var i = 1; i < searchKeys.Count; i++)
                {
                    var currentSearch = searchKeys[i];
                    GetSubsetOfNode(foundNode, currentSearch, featureValue);
                }
            }

            return foundNode;
        }

        /// <summary>
        /// Queries the node according to the search key.
        /// </summary>
        /// <param name="searchKey">The search key.</param>
        /// <returns>Returns the found node.</returns>
        private INode QueryNode((string featureName, string featureValue) searchKey)
        {
            var tmpNode = Root;
            var foundNode = new Node();

            if (tmpNode.Feature == searchKey.featureName)
            {
                foundNode.Feature = tmpNode.Feature;
                var childNode = tmpNode.Children[searchKey.featureValue];
                foundNode.Children.Add(searchKey.featureValue, childNode);
            }

            return foundNode;
        }

        /// <summary>
        /// Gets the sub set of the requested node according to the search key.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="searchKey">The search key.</param>
        /// <param name="rootKey">The root.</param>
        private void GetSubsetOfNode(INode node, (string featureName, string featureValue) searchKey, string rootKey)
        {
            var subset = node.Children.Where(s => s.Value.Children.ContainsKey(searchKey.featureValue))
                        .ToDictionary(dict => dict.Key, dict => dict.Value);

            // we have to get the keys which are not asked from the children dictionary.
            var toDeleteKeys = subset[rootKey].Children.Keys.Where(x => x != searchKey.featureValue).ToList();
            foreach (var toDelete in toDeleteKeys)
            {
                // each key should be removed since we are looking for searchKey.featureValue
                subset[rootKey].Children.Remove(toDelete);
            }
        }
    }
}
