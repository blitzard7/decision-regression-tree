using DecisionTree.Logic.Models;
using System;

namespace DecisionTree.Logic.Trees
{
    public class RegressionTree : IRegressionTree
    {
        public INode Root => throw new NotImplementedException();

        public ITree BuildTree(CsvData data)
        {
            throw new NotImplementedException();
        }

        public INode Query(string searchKeys)
        {
            throw new NotImplementedException();
        }
    }
}
