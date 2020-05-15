using DecisionTree.Logic.Models;
using System.Collections.Generic;

namespace DecisionTree.Logic.Interfaces
{
    /// <summary>
    /// Represents the INode interface.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        INode Parent { get; set; }

        /// <summary>
        /// Gets a value indicating whether the node is leaf or not.
        /// </summary>
        bool IsLeaf { get; }

        /// <summary>
        /// Gets the feature name.
        /// </summary>
        string Feature { get; }

        /// <summary>
        /// Gets the value of the current feature.
        /// </summary>
        string FeatureValue { get; }

        /// <summary>
        /// Gets the total entropy.
        /// </summary>
        double TotalEntropy { get; }

        /// <summary>
        /// Gets the node entropy.
        /// </summary>
        double NodeEntropy { get; }

        /// <summary>
        /// Gets the information gain of the node.
        /// </summary>
        double NodeInformationGain { get; }

        /// <summary>
        /// Gets the result value.
        /// </summary>
        string Result { get; }

        /// <summary>
        /// Gets the current classification containing distinct result values and its occurrence.
        /// </summary>
        Dictionary<string, int> CurrentClassification { get; }

        /// <summary>
        /// Gets or sets the children containing the feature value as key and the pointing node.
        /// </summary>
        Dictionary<string, INode> Children { get; set; }

        /// <summary>
        /// Starts calculating the tree.
        /// </summary>
        /// <param name="data">The input data.</param>
        void Start(CsvData data);

        /// <summary>
        /// Builds the tree recursively.
        /// </summary>
        /// <param name="data">The input data.</param>
        void Build(CsvData data);

        /// <summary>
        /// Checks whether the current split data contains only homogeneous data or not.
        /// </summary>
        /// <param name="data">The split data.</param>
        /// <returns>Returns whether the current split data is homogeneous or not.</returns>
        bool ContainsHomogeneousData(CsvData data);
    }
}
