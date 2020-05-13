using DecisionTree.Logic.Models;
using System.Collections.Generic;

namespace DecisionTree.Logic.Trees
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
        Dictionary<string, int> CurrentClassification { get; }
        Dictionary<string, INode> Children { get; set; }
        void Start(CsvData data);
        void Build(CsvData data);
        bool ContainsHomogeneousData(CsvData data);
    }
}
