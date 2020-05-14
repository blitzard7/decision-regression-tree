using DecisionTree.Logic.Helper;
using DecisionTree.Logic.Interfaces;
using DecisionTree.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Logic.Trees
{
    public class Node : INode
    {
        public Node()
        {
            Children = new Dictionary<string, INode>();
            CurrentClassification = new Dictionary<string, int>();
        }

        public string Feature { get; set; }

        public INode Parent { get; set; }

        public Dictionary<string, INode> Children { get; set; }

        public bool IsLeaf => Children.Values.Count == 0;

        public string Result { get; set; }

        public Dictionary<string, int> CurrentClassification { get; set; }

        public double NodeEntropy { get; private set; }

        public double NodeInformationGain { get; private set; }

        public double TotalEntropy { get; private set; }

        public string FeatureValue { get; private set; }

        public void Start(CsvData data)
        {
            TotalEntropy = data.EG;
            Build(data);
        }
      
        public void Build(CsvData data)
        {
            var currentDistinctResultSet = data.ResultSetValues.Distinct();
            CurrentClassification = currentDistinctResultSet.CalculateOccurenceOfGivenEntries(data.ResultSetValues);

            if (ContainsHomogeneousData(data))
            {
                return;
            }

            var feature = CalculateFeature(data);
            var featureValuesDistinct = feature.Values.Distinct();
            Feature = feature.Name;

            foreach (var distinctValue in featureValuesDistinct)
            {
                var node = new Node
                {
                    Parent = this,
                    FeatureValue = distinctValue,
                    TotalEntropy = TotalEntropy,
                };

                // TODO: take care of correct placing NodeEntropy
                // What happens with subFeatureEntropy? -> subfeatureEntropy != NodeEntropy
                NodeEntropy = feature.FeatureEntropy;
                NodeInformationGain = feature.FeatureInformationGain;

                var currentData = data.Filter(feature.Name, distinctValue);
                // calculate current classification (from current data set calculate how many times each distinct resultValue occurs)
                // If feature selected, the subset of csvdata should not contain the actual feature in header anymore.
                node.Build(currentData);
                this.Children.Add(distinctValue, node);
            }

            return;
        }

        public bool ContainsHomogeneousData(CsvData data)
        {
            // Check if rows contains only homogeneous data.
            // need to calculate the entropy 
            var distinctResultValues = data.GetUniqueColumnValues(data.ResultCategory).ToList();
            if (distinctResultValues.Count == 0)
            {
                throw new ArgumentException("Result values where empty.");
            }

            if (distinctResultValues.Count == 1 || data.Headers.Count == 1)
            {
                // data.headers.count == 1 -> represents the ResultCategory therefore we have homogeneous data.
                Result = distinctResultValues[0];
                return true;
            }

            return false;
        }

        private Feature CalculateFeature(CsvData data)
        {
            var informationGainOfFeatures = CalculateInformationGainOfFeatures(data).ToList();

            // We have to calculate for each feature the IG and then select MAX(FeatureIG).
            var (featureName, featureEntropy, featureIG) = SelectFeatureForSplit(informationGainOfFeatures);
            var toSelectFeatureValues = data.Columns[featureName];
            var feature = new Feature()
            {
                Name = featureName,
                Values = toSelectFeatureValues,
                FeatureEntropy = featureEntropy,
                FeatureInformationGain = featureIG
            };

            return feature;
        }

        private IEnumerable<(string featureName, double featureEntropy, double featureIG)> CalculateInformationGainOfFeatures(CsvData data)
        {
            var informationGainOfFeatures = new List<(string, double, double)>();

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

                var eCurrent = CalculateEntropyOfFeature(dataSetOfCurrentColumn, data.ResultSetValues);
                var igCurrent = Calculation.CalculateInformationGain(TotalEntropy, eCurrent);

                informationGainOfFeatures.Add((currentColumn, eCurrent, igCurrent));
            }

            return informationGainOfFeatures;
        }

        private double CalculateEntropyOfFeature(Dictionary<string, List<string>> dataSetOfCurrentColumn, List<string> resultSetValues)
        {
            var distinctResultSetValues = resultSetValues.Distinct().ToList();
            var entropyOfSubClasses = CalculateEntropyOfFeatureSubclasses(dataSetOfCurrentColumn, distinctResultSetValues);
            var totalAmount = resultSetValues.Count;
            double entropyOfFeature = 0;

            foreach (var key in dataSetOfCurrentColumn.Keys)
            {
                var occurences = dataSetOfCurrentColumn[key].Count;
                entropyOfFeature += ((double)occurences / totalAmount) * entropyOfSubClasses[key];
            }
            
            return entropyOfFeature;
        }

        private Dictionary<string, double> CalculateEntropyOfFeatureSubclasses(Dictionary<string, List<string>> dataSetOfCurrentColumn, List<string> distinctResultSetValues)
        {
            var entropyOfEachSubClass = new Dictionary<string, double>();
            double entropyOfCurrentFeature = 0;
            foreach (var key in dataSetOfCurrentColumn.Keys)
            {
                var currentDataSet = dataSetOfCurrentColumn[key];
                var eCurrent = Calculation.CalculateEntropy(distinctResultSetValues, currentDataSet);
                entropyOfCurrentFeature += eCurrent;
                entropyOfEachSubClass.Add(key, eCurrent);
            }

            return entropyOfEachSubClass;
        }

        private (string featureName, double featureEntropy, double featureIG) SelectFeatureForSplit(List<(string featureName, double featureEntropy, double featureIG)> informationGainOfFeatures)
        {
            // Select MAX(FeatureIG) of IG collection.
            // If n elements have the same highest IG, then select first occurence of it.
            var toSelectFeature = informationGainOfFeatures.Max(x => x.featureIG);
            return informationGainOfFeatures.First(x => x.featureIG == toSelectFeature);
        }
    }
}
