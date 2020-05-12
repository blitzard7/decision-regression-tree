using System.Collections.Generic;

namespace DecisionTree.Logic.Trees
{
    public interface IDecisionTree : ITree
    {
        void PostPruning();
    }
}
