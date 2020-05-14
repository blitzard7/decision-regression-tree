using DecisionTree.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionTree.Logic.Trees
{
    public interface IDecisionTreeNode
    {
        double TotalEntropy { get; }
        double NodeEntropy { get; }
        double NodeInformationGain { get; }
        Dictionary<string, int> CurrentClassification { get; }
    }
}
