using DecisionTree.Logic.Models;

namespace DecisionTree.Logic.Trees
{
    public interface ITree
    {
        INode Root { get; }
        INode Query(string searchKeys);
        ITree BuildTree(CsvData data);
    }
}
