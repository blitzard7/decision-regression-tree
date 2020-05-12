using DecisionTree.Logic.Models;
using System;
using System.Collections.Generic;

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

        public INode Query(List<(string featureName, string featureValue)> searchKeys)
        {
            throw new NotImplementedException();
        }
    }
}
