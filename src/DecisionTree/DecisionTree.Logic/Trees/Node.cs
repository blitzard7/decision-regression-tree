using System.Collections.Generic;

namespace DecisionTree.Logic.Trees
{
    public class Node : INode
    {
        public Node()
        {
            Children = new Dictionary<string, INode>();
        }

        public string SearchString { get; set; }
        public INode Parent { get; set; }
        public Dictionary<string, INode> Children { get; set; }
        public bool IsLeaf => Children.Values.Count == 0;
    }
}
