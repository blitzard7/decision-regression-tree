namespace DecisionTree.Logic.Trees
{
    public interface IDecisionTree : ITree
    {
        decimal CalculateEntropy();
        decimal CalculateInformationGain();
    }
}
