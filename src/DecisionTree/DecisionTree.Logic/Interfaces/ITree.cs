using DecisionTree.Logic.Models;
using System.Collections.Generic;

namespace DecisionTree.Logic.Interfaces
{
    /// <summary>
    /// Represents the ITree node.
    /// </summary>
    public interface ITree
    {
        /// <summary>
        /// Gets the root node.
        /// </summary>
        INode Root { get; }

        /// <summary>
        /// Queries the constructed tree with the given search keys.
        /// </summary>
        /// <param name="searchKeys">The search keys.</param>
        /// <returns>Returns the resulted node.</returns>
        INode Query(List<(string featureName, string featureValue)> searchKeys);
        
        /// <summary>
        /// Builds the tree according to the <see cref="CsvData"/> recursively.
        /// </summary>
        /// <param name="data">The csv data.</param>
        /// <returns>Returns the constructed tree.</returns>
        ITree BuildTree(CsvData data);
    }
}
