using DecisionTree.Logic.Models;
using System.Collections.Generic;

namespace DecisionTree.Logic.Trees
{
    public interface INode
    {
        INode Parent { get; set; }
        bool IsLeaf { get; }
        string SearchString { get; set; }
        Dictionary<string, INode> Children { get; set; }
        INode Build(CsvData data);
        bool ContainsHomogeneousData(INode node);
    }
}
