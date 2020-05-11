using DecisionTree.Logic.Models;
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


        public static IEnumerable<(string, double)> CalculateInformationGainOfFeatures(CsvData data, double totalEntropy, List<string> resultSetValues)
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

                var eCurrent = CalculateEntropyOfFeature(dataSetOfCurrentColumn, resultSetValues);
                var igCurrent = CalculateInformationGain(totalEntropy, eCurrent);

                informationGainOfFeatures.Add((currentColumn, igCurrent));
            }

            return informationGainOfFeatures;
        }

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
