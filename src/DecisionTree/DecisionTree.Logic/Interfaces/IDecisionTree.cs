using System.Collections.Generic;

namespace DecisionTree.Logic.Interfaces
{
    public interface IDecisionTree : ITree
    {
        void PostPruning();
    }
}
