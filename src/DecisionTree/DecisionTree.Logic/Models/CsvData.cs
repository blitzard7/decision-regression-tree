using DecisionTree.Logic.Helper;
using DecisionTree.Logic.Validator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Logic.Models
{
    /// <summary>
    /// Represents the CsvData class.
    /// </summary>
    public class CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvData"/> class.
        /// </summary>
        public CsvData()
        {
            Columns = new Dictionary<string, List<string>>();
            Rows = new List<string>();
            Headers = new List<string>();
        }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        public Dictionary<string, List<string>> Columns { get; set; }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        public List<string> Rows { get; set; }

        /// <summary>
        /// Gets or sets the headers, which are represented as column names.
        /// </summary>
        public List<string> Headers { get; set; }

        /// <summary>
        /// Gets the current result set values.
        /// </summary>
        public List<string> ResultSetValues => GetResultSetValues();

        /// <summary>
        /// Gets the result category.
        /// </summary>
        public string ResultCategory => Headers.Count > 0 ? Headers[^1] : string.Empty;

        /// <summary>
        /// Gets the entropy of the table.
        /// </summary>
        public double Eg => CalculateEntropyOfTable();

        /// <summary>
        /// Filters the current csv data according to the requested feature and its distinct value.
        /// </summary>
        /// <param name="featureName">The requested feature.</param>
        /// <param name="distinctValue">The requested distinct feature value.</param>
        /// <returns>Returns a subset of the csv data.</returns>
        public CsvData Filter(string featureName, string distinctValue)
        {
            // collect row data for given distinctValue.
            var columns = new Dictionary<string, List<string>>();

            // 1. collect headers where x != featurename
            // 2. collect all rows containing distinctValue
            // 3. create subset containing relevant headers with corresponing rows 
            //  rows should not contain distinctValue

            var featureIndex = this.Headers.IndexOf(featureName);
            var headers = TakeColumnsExcludingCurrentFeature(featureName);

            var relevantRows = TakeRelevantRowsFromCurrentFeature(this.Rows, featureIndex, distinctValue);

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

        /// <summary>
        /// Calculates the entropy of the table.
        /// </summary>
        /// <returns>Returns the entropy.</returns>
        private double CalculateEntropyOfTable()
        {
            var distinctResultValues = this.GetUniqueColumnValues(ResultCategory).ToList();
            return Calculation.CalculateEntropy(distinctResultValues, ResultSetValues);
        }

        /// <summary>
        /// Gets the result set values.
        /// </summary>
        /// <returns>Returns the result set values.</returns>
        private List<string> GetResultSetValues()
        {
            return Columns[ResultCategory];
        }

        /// <summary>
        /// Take columns without the current requested feature.
        /// </summary>
        /// <param name="feature">The requested feature.</param>
        /// <returns>Returns the column names excluding the requested feature.</returns>
        private List<string> TakeColumnsExcludingCurrentFeature(string feature)
        {
            var tmpHeaders = new List<string>();
            tmpHeaders.AddRange(Headers);
            tmpHeaders.Remove(feature);
            return tmpHeaders;
        }

        private List<string> TakeRelevantRowsFromCurrentFeature(IEnumerable<string> rows, int featureIndex, string distinctValue)
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
