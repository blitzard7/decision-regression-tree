using DecisionTree.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Logic.Trees
{
    /* TODO:
     * CalculateEntropy, IG and Split should be placed in Node
     */ 

    public class DecisionTree : IDecisionTree
    {
        private readonly INode _root = new Node();
        private readonly List<string> _resultSetValues = new List<string>();
        private readonly List<string> _distinctResultValues = new List<string>();

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
            _resultSetValues.AddRange(data.Columns[resultCategory]);
            var distinctResultValues = data.GetUniqueColumnValues(resultCategory).ToList();
            _distinctResultValues.AddRange(distinctResultValues);

            var eg = CalculateEntropy(_distinctResultValues, _resultSetValues);
            var informationGainOfFeatures = CalculateInformationGainOfFeatures(data, eg).ToList();

            // We have to calculate for each feature the IG and then select MAX(FeatureIG).
            var toSelectFeature = SelectFeatureForSplit(informationGainOfFeatures);

            // After getting feature for SPLIT, we have to Create the node
            return null;
        }

        public double CalculateEntropy(List<string> distinctResultValues, List<string> resultSetValues)
        {
            const int logBase = 2;
            var totalAmount = resultSetValues.Count;
            var occurences = distinctResultValues.CalculateOccurenceOfGivenEntries(resultSetValues);
            var entropy = occurences.Sum(x => -((double)x.Value / totalAmount) * Math.Log(((double)x.Value / totalAmount), logBase));

            return entropy;
        }

        public double CalculateInformationGain(double totalEntropy, double featureEntropy) => totalEntropy - featureEntropy;

        public INode Query(string searchKeys)
        {
            // After building the tree, Node is queryable like: sunny,high,hot -> should return one possibility 
            throw new System.NotImplementedException();
        }

        private IEnumerable<(string, double)> CalculateInformationGainOfFeatures(CsvData data, double totalEntropy)
        {
            var informationGainOfFeatures = new List<(string, double)>();
            
            // Next step is to calculate Entropy for each column and possibility 
            // data.Headers.Count - 1, since we do not want to iterate over the resultcaterogy.
            for (int i = 0; i < data.Headers.Count - 1; i++)
            {
                // DataSet contains for given column name, all rows depending on column distinct values.
                // This will be relevant for calculating Entropy for each column entry of row.
                var dataSetOfCurrentColumn = new Dictionary<string, List<string>>();
                var currentColumn = data.Headers[i];
                var distinctColumValues = data.GetUniqueColumnValues(currentColumn);

                foreach (var val in distinctColumValues)
                {
                    var specificRows = data.GetSpecificRowEntries(val).ToList();
                    dataSetOfCurrentColumn.Add(val, specificRows);
                }

                var eCurrent = CalculateEntropyOfFeature(dataSetOfCurrentColumn);
                var igCurrent = CalculateInformationGain(totalEntropy, eCurrent);

                informationGainOfFeatures.Add((currentColumn, igCurrent));
            }

            return informationGainOfFeatures;
        }

        private double CalculateEntropyOfFeature(Dictionary<string, List<string>> dataSetOfCurrentColumn)
        {
            var entropyOfSubClasses = CalculateEntropyOfFeatureSubclasses(dataSetOfCurrentColumn);
            var totalAmount = _resultSetValues.Count;
            double entropyOfFeature = 0;
            
            foreach (var key in dataSetOfCurrentColumn.Keys)
            {
                var occurences = dataSetOfCurrentColumn[key].Count;
                entropyOfFeature += ((double)occurences / totalAmount) * entropyOfSubClasses[key];
            }

            return entropyOfFeature;
        }

        private Dictionary<string, double> CalculateEntropyOfFeatureSubclasses(Dictionary<string, List<string>> dataSetOfCurrentColumn)
        {
            var entropyOfEachSubClass = new Dictionary<string, double>();
            double entropyOfCurrentFeature = 0;
            foreach (var key in dataSetOfCurrentColumn.Keys)
            {
                var currentDataSet = dataSetOfCurrentColumn[key];
                var eCurrent = CalculateEntropy(_distinctResultValues, currentDataSet);
                entropyOfCurrentFeature += eCurrent;
                entropyOfEachSubClass.Add(key, eCurrent);
            }

            return entropyOfEachSubClass;
        }

        private (string, double) SelectFeatureForSplit(List<(string, double)> informationGainOfFeatures)
        {
            // Select MAX(FeatureIG) of IG collection.
            // If n elements have the same highest IG, then select first occurence of it.
            var toSelectFeature = informationGainOfFeatures.Max(x => x.Item2);
            return informationGainOfFeatures.First(x => x.Item2 == toSelectFeature);
        }

        private void AddNode()
        {
            // Adding nodes should be done recursively?
            throw new System.NotImplementedException();
        }
    }
}
