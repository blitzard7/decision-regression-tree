using System.Collections.Generic;
using DecisionTree.Logic.Models;

namespace DecisionTree.Logic.Interfaces
{
    public interface INode
    {
        INode Parent { get; set; }

        bool IsLeaf { get; }

        string Feature { get; }

        string FeatureValue { get; }

        double TotalEntropy { get; }

        double NodeEntropy { get; }

        double NodeInformationGain { get; }

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
