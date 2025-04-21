using System.Collections.Generic;
using DecisionTree.Logic.Models;

namespace DecisionTree.Logic.Interfaces
{
    public interface ITree
    {
        INode Root { get; }

        /// <summary>
        /// Queries the constructed tree with the given search keys.
        /// </summary>
        /// <param name="searchKeys">The search keys.</param>
        /// <returns>Returns the resulted node.</returns>
        INode Query(Dictionary<string, string> searchKeys);
        
        /// <summary>
        /// Builds the tree according to the <see cref="CsvData"/> recursively.
        /// </summary>
        /// <param name="data">The csv data.</param>
        /// <returns>Returns the constructed tree.</returns>
        ITree BuildTree(CsvData data);
    }
}
