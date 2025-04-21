using DecisionTree.Logic.Helper;
using DecisionTree.Logic.Validator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Logic.Models
{
    public class CsvData
    {
        public CsvData()
        {
            Columns = new Dictionary<string, List<string>>();
            Rows = new List<string>();
            Headers = new List<string>();
        }

        public Dictionary<string, List<string>> Columns { get; set; }

        public List<string> Rows { get; set; }

        public List<string> Headers { get; set; }

        /// <summary>
        /// Gets the current result set values.
        /// </summary>
        public List<string> ResultSetValues => GetResultSetValues();

        public string ResultCategory => Headers.Count > 0 ? Headers[^1] : string.Empty;

        /// <summary>
        /// Gets the entropy of the table.
        /// </summary>
        public double TableEntropy => CalculateEntropyOfTable();

        /// <summary>
        /// Filters the current csv data according to the requested feature and its distinct value.
        /// </summary>
        /// <param name="featureName">The requested feature.</param>
        /// <param name="distinctValue">The requested distinct feature value.</param>
        /// <returns>Returns a subset of the csv data.</returns>
        public CsvData Filter(string featureName, string distinctValue)
        {
            // 1. collect headers where header != featureName
            // 2. collect all rows containing distinctValue
            // 3. create subset containing relevant headers with corresponding rows 
            //  rows should not contain distinctValue
            
            var columns = new Dictionary<string, List<string>>();
            var featureIndex = this.Headers.IndexOf(featureName);
            var headers = ExtractHeadersWithoutFeature(featureName);

            var relevantRows = ExtractRelevantRowsByFeatureValue(this.Rows, featureIndex, distinctValue);

            foreach (var header in headers)
            {
                var headerIndex = headers.IndexOf(header);
                var columnValues = GetColumnValues(relevantRows, headerIndex);
                columns.Add(header, columnValues);
            }

            var dataSubSet = new CsvData
            {
                Columns = columns,
                Rows = relevantRows,
                Headers = headers
            };

            return dataSubSet;
        }

        private double CalculateEntropyOfTable()
        {
            var distinctResultValues = this.GetUniqueColumnValues(ResultCategory).ToList();
            return Calculation.CalculateEntropy(distinctResultValues, ResultSetValues);
        }

        private List<string> GetResultSetValues() => Columns[ResultCategory];  

        /// <summary>
        /// Take columns without the current requested feature.
        /// </summary>
        /// <param name="feature">The requested feature.</param>
        /// <returns>Returns the column names excluding the requested feature.</returns>
        private List<string> ExtractHeadersWithoutFeature(string feature)
        {
            var tmpHeaders = new List<string>();
            tmpHeaders.AddRange(Headers);
            tmpHeaders.Remove(feature);
            return tmpHeaders;
        }

        /// <summary>
        /// Takes relevant rows from the current distinct feature value.
        /// The subset of the selected rows should not contain the distinct feature values.
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <param name="featureIndex">The feature index.</param>
        /// <param name="distinctValue">The distinct feature value.</param>
        /// <returns>Returns a subset of the rows containing the rows from the feature index excluding the distinct feature value.</returns>
        private List<string> ExtractRelevantRowsByFeatureValue(IEnumerable<string> rows, int featureIndex, string distinctValue)
        {
            var relevantRows = new List<string>();

            foreach (var currentRow in rows)
            {
                var split = currentRow.Split(FormValidator.ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries).ToList();
                
                if (split[featureIndex] == distinctValue)
                {
                    split[featureIndex] = string.Empty;
                    split.RemoveAll(string.IsNullOrEmpty);
                    var newRow = string.Join(FormValidator.ValidValueSeparator, split).Trim();
                    relevantRows.Add(newRow);
                }
            }

            return relevantRows;
        }

        /// <summary>
        /// Gets the row values for current column index.
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <param name="headerIndex">The current column index.</param>
        /// <returns>Returns a sub set of the columns for the requested feature.</returns>
        private List<string> GetColumnValues(IEnumerable<string> rows, int headerIndex)
        {
            return rows.Select(x =>
            {
                var split = x.Split(FormValidator.ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries);
                var selected = split[headerIndex];
                return selected;
            }).ToList();
        }
    }
}
