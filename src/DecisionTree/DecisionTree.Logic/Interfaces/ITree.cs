using DecisionTree.Logic.Models;
using System.Collections.Generic;

namespace DecisionTree.Logic.Interfaces
{
    public interface ITree
    {
        INode Root { get; }
        INode Query(List<(string featureName, string featureValue)> searchKeys);
        ITree BuildTree(CsvData data);
    }
}
