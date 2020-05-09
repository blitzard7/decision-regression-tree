using DecisionTree.Logic.Models;
using System;
using System.Linq;

namespace DecisionTree.Logic.Trees
{
    public class DecisionTree : IDecisionTree
    {
        private readonly INode _root = new Node();

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
            var resultCategory = data.Headers[^1];
            var resultValues = data.Columns[resultCategory];
            var distictResultValues = data.GetUniqueColumnValues(resultCategory).ToList();
            var totalAmount = resultValues.Count;

            var occurences = distictResultValues.CalculateOccurenceOfGivenEntries(resultValues);
            var eg = occurences.Sum(x => -((double)x.Value / totalAmount) * Math.Log(((double)x.Value / totalAmount), 2));

            return null;
        }

        public decimal CalculateEntropy()
        {
            throw new System.NotImplementedException();
        }

        public decimal CalculateInformationGain()
        {
            throw new System.NotImplementedException();
        }

        public INode Query(string searchKeys)
        {
            // After building the tree, Node is queryable like: sunny,high,hot -> should return one possibility 
            throw new System.NotImplementedException();
        }

        private void AddNode()
        {
            throw new System.NotImplementedException();
        }
    }
}
