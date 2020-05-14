using System.Collections.Generic;

namespace DecisionTree.Logic.Interfaces
{
    public interface IDecisionTreeNode
    {
        double TotalEntropy { get; }
        double NodeEntropy { get; }
        double NodeInformationGain { get; }
        Dictionary<string, int> CurrentClassification { get; }
    }
}
