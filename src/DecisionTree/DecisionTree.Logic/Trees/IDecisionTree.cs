using System.Collections.Generic;

namespace DecisionTree.Logic.Trees
{
    public interface IDecisionTree : ITree
    {
        double CalculateEntropy(List<string> distinctResultValues, List<string> resultValues);
        double CalculateInformationGain(double totalEntropy, double featureEntropy);
    }
}
