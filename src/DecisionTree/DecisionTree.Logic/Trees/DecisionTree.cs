using DecisionTree.Logic.Helper;
using DecisionTree.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace DecisionTree.Logic.Trees
{
    /* TODO:
     * CalculateEntropy, IG and Split should be placed in Node
     */ 

    public class DecisionTree : IDecisionTree
    {
        public INode Root { get; private set; }

        // Build tree from given CsvData
        // Recursive Task?
        // Entropy and InformationGain is calculated 
        // Stop groving after all leafe nodes are homogeneouse (Post-pruning)
        // Returns DecisionTree
        public ITree BuildTree(CsvData data)
        {
            // 1. determine root node (column) of tree
            // 2. calculate for each search key of current node (first node is root) the entropy and IG for next node (child)
            // 3. repeat 2 until no split is needed (meaning that all leafe nodes contain homogeneous data)

            // Assumption: Last column represents query result.
            // For E(G) we have to extract all result values 
            Root = new Node();
            Root.Build(data);
            // After getting feature for SPLIT, we have to Create the node
            return this;
        }

        public void PostPruning()
        {
            throw new NotImplementedException();
        }

        public INode Query(string searchKeys)
        {
            throw new NotImplementedException();
        }
    }
}
