using DecisionTree.Logic.Models;

namespace DecisionTree.Logic.Trees
{
    public interface ITree
    {
        INode Query(string searchKeys);
        ITree BuildTree(CsvData data);
    }
}
