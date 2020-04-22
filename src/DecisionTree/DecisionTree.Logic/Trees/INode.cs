namespace DecisionTree.Logic.Trees
{
    public interface INode
    {
        INode Parent { get; set; }
        INode Child { get; set; }
        bool IsLeaf { get; }
    }
}
