using System.Collections.Generic;

namespace DecisionTree.Logic.Trees
{
    public interface INode
    {
        INode Parent { get; set; }
        bool IsLeaf { get; }
        string SearchString { get; set; }
        Dictionary<string, INode> Children { get; set; }
    }
}
