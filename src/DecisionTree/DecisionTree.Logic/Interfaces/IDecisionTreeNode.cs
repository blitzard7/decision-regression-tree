using System.Collections.Generic;

namespace DecisionTree.Logic.Interfaces
{
    /// <summary>
    /// Represents the IDecisionTreeNode interface.
    /// </summary>
    public interface IDecisionTreeNode
    {
        /// <summary>
        /// Gets the total entropy.
        /// </summary>
        double TotalEntropy { get; }

        /// <summary>
        /// Gets the node entropy.
        /// </summary>
        double NodeEntropy { get; }

        /// <summary>
        /// Gets the node information gain.
        /// </summary>
        double NodeInformationGain { get; }

        /// <summary>
        /// Gets the current classification containing the distinct result value as key and its occurrences.
        /// </summary>
        Dictionary<string, int> CurrentClassification { get; }
    }
}
