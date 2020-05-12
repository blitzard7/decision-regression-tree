using DecisionTree.Logic.Models;
using System.Collections.Generic;

namespace DecisionTree.Logic.Trees
{
    public interface INode
    {
        INode Parent { get; set; }
        bool IsLeaf { get; }
        string Feature { get; }
        Dictionary<string, INode> Children { get; set; }
        void Start(CsvData data);
        void Build(CsvData data);
        bool ContainsHomogeneousData(CsvData data);
    }
}
