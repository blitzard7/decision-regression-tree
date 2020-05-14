using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Logic.Helper
{
    public static class Calculation
    {
        public static double CalculateEntropy(List<string> distinctResultValues, List<string> resultSetValues)
        {
            const int logBase = 2;
            var totalAmount = resultSetValues.Count;
            var occurences = distinctResultValues.CalculateOccurenceOfGivenEntries(resultSetValues);
            var entropy = occurences.Sum(x => -((double)x.Value / totalAmount) * Math.Log(((double)x.Value / totalAmount), logBase));

            return entropy;
        }

        public static double CalculateInformationGain(double totalEntropy, double featureEntropy) => totalEntropy - featureEntropy;

        public static double CalculateEntropyOfFeature(Dictionary<string, List<string>> dataSetOfCurrentColumn, List<string> resultSetValues)
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

        public static Dictionary<string, double> CalculateEntropyOfFeatureSubclasses(Dictionary<string, List<string>> dataSetOfCurrentColumn, List<string> distinctResultSetValues)
        {
            var entropyOfEachSubClass = new Dictionary<string, double>();
            double entropyOfCurrentFeature = 0;
            foreach (var key in dataSetOfCurrentColumn.Keys)
            {
                var currentDataSet = dataSetOfCurrentColumn[key];
                var eCurrent = CalculateEntropy(distinctResultSetValues, currentDataSet);
                entropyOfCurrentFeature += eCurrent;
                entropyOfEachSubClass.Add(key, eCurrent);
            }

            return entropyOfEachSubClass;
        }
    }
}
