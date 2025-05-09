﻿using DecisionTree.Logic.Helper;
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

        /// <summary>
        /// Gets or sets the children containing the feature value as key and the pointing node.
        /// </summary>
        public Dictionary<string, INode> Children { get; set; }

        /// <summary>
        /// Gets a value indicating whether the node is leaf or not.
        /// </summary>
        public bool IsLeaf => Children.Values.Count == 0;

        public string Result { get; set; }

        /// <summary>
        /// Gets or sets the current classification containing distinct result values and its occurrence.
        /// </summary>
        public Dictionary<string, int> CurrentClassification { get; set; }

        public double NodeEntropy { get; private set; }

        public double NodeInformationGain { get; private set; }

        public double TotalEntropy { get; private set; }

        public string FeatureValue { get; private set; }

        public void Start(CsvData data)
        {
            TotalEntropy = data.TableEntropy;
            Build(data);
        }

        /// <summary>
        /// Builds the tree recursively.
        /// </summary>
        /// <param name="data">The input data.</param>
        public void Build(CsvData data)
        {
            var currentDistinctResultSet = data.ResultSetValues.Distinct();
            CurrentClassification = currentDistinctResultSet.CalculateOccurenceOfGivenEntries(data.ResultSetValues);

            // if current subset of data contains homogeneous data then return current node,
            // since current node is leaf node containing the result.
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

                NodeEntropy = feature.FeatureEntropy;
                NodeInformationGain = feature.FeatureInformationGain;

                var currentData = data.Filter(feature.Name, distinctValue);
                node.Build(currentData);
                this.Children.Add(distinctValue, node);
            }

            return;
        }

        /// <summary>
        /// Checks whether the current split data contains only homogeneous data or not.
        /// </summary>
        /// <param name="data">The split data.</param>
        /// <returns>Returns whether the current split data is homogeneous or not.</returns>
        public bool ContainsHomogeneousData(CsvData data)
        {
            var distinctResultValues = data.GetUniqueColumnValues(data.ResultCategory).ToList();
            
            if (distinctResultValues.Count == 0)
            {
                throw new ArgumentException("Result values where empty.");
            }

            // if our distinct result values contains one entry or
            // the subset of data contains one header element (if one header then this header represents the result category column)
            // then we have reached homogeneous data.
            if (distinctResultValues.Count == 1 || data.Headers.Count == 1)
            {
                Result = distinctResultValues[0];
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculates the feature.
        /// </summary>
        /// <param name="data">The csv data corresponding the current feature.</param>
        /// <returns>Returns the feature.</returns>
        private Feature CalculateFeature(CsvData data)
        {
            // calculate the information gain of all features and return per feature the feature name, current entropy and calculated information gain.
            var informationGainOfFeatures = CalculateInformationGainOfFeatures(data).ToList();

            // We have to calculate for each feature the IG and then select MAX(FeatureIG).
            var (featureName, featureEntropy, featureIg) = SelectFeatureForSplit(informationGainOfFeatures);
            var toSelectFeatureValues = data.Columns[featureName];
            var feature = new Feature()
            {
                Name = featureName,
                Values = toSelectFeatureValues,
                FeatureEntropy = featureEntropy,
                FeatureInformationGain = featureIg
            };

            return feature;
        }

        /// <summary>
        /// Calculates the information gain of the current feature.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Returns a list containing information of the feature name with its entropy and information gain.</returns>
        private IEnumerable<(string featureName, double featureEntropy, double featureIg)> CalculateInformationGainOfFeatures(CsvData data)
        {
            var informationGainOfFeatures = new List<(string, double, double)>();

            // Next step is to calculate Entropy for each column and possibility 
            // data.Headers.Count - 1, since we do not want to iterate over the result category.
            for (var i = 0; i < data.Headers.Count - 1; i++)
            {
                // DataSet contains for given column name, all rows depending on column distinct values.
                // This will be relevant for calculating Entropy for each column entry of row.
                var dataSetOfCurrentColumn = new Dictionary<string, List<string>>();
                var currentColumn = data.Headers[i];
                var uniqueColumnValues = data.GetUniqueColumnValues(currentColumn);

                foreach (var val in uniqueColumnValues)
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

        /// <summary>
        /// Calculates the entropy of the feature.
        /// </summary>
        /// <param name="dataSetOfCurrentColumn">The data set of current feature column.</param>
        /// <param name="resultSetValues">The result set.</param>
        /// <returns>Returns the entropy of the feature.</returns>
        private double CalculateEntropyOfFeature(Dictionary<string, List<string>> dataSetOfCurrentColumn, IReadOnlyCollection<string> resultSetValues)
        {
            var distinctResultSetValues = resultSetValues.Distinct().ToList();
            var entropyOfSubClasses =
                CalculateEntropyOfFeatureSubclasses(dataSetOfCurrentColumn, distinctResultSetValues);
            var totalAmount = resultSetValues.Count;
            double entropyOfFeature = 0;

            foreach (var key in dataSetOfCurrentColumn.Keys)
            {
                var occurrences = dataSetOfCurrentColumn[key].Count;
                entropyOfFeature += ((double) occurrences / totalAmount) * entropyOfSubClasses[key];
            }

            return entropyOfFeature;
        }

        /// <summary>
        /// Calculates the entropy of each feature value.
        /// </summary>
        /// <param name="dataSetOfCurrentColumn">The data set of current feature column.</param>
        /// <param name="distinctResultSetValues">The distinct result set values.</param>
        /// <returns>Returns a dictionary containing the feature value as key and its corresponding entropy.</returns>
        private Dictionary<string, double> CalculateEntropyOfFeatureSubclasses(
            Dictionary<string, List<string>> dataSetOfCurrentColumn, List<string> distinctResultSetValues)
        {
            var entropyOfEachSubClass = new Dictionary<string, double>();
            foreach (var key in dataSetOfCurrentColumn.Keys)
            {
                var currentDataSet = dataSetOfCurrentColumn[key];
                var eCurrent = Calculation.CalculateEntropy(distinctResultSetValues, currentDataSet);
                entropyOfEachSubClass.Add(key, eCurrent);
            }

            return entropyOfEachSubClass;
        }

        /// <summary>
        /// Selects the feature with the maximum information gain for tree split.
        /// </summary>
        /// <param name="informationGainOfFeatures">The features with the information gain.</param>
        /// <returns>Returns the feature with the maximum information gain.</returns>
        private (string featureName, double featureEntropy, double featureIg) SelectFeatureForSplit(
            IReadOnlyCollection<(string featureName, double featureEntropy, double featureIg)> informationGainOfFeatures)
        {
            // Select MAX(FeatureIG) of IG collection.
            // If n elements have the same highest IG, then select first occurence of it.
            var toSelectFeature = informationGainOfFeatures.Max(x => x.featureIg);
            return informationGainOfFeatures.First(x => x.featureIg == toSelectFeature);
        }
    }
}