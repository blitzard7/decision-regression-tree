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
        public List<string> ResultSetValues { get => GetResultSetValues(); }
        public string ResultCategory
        {
            get
            {
                if (this.Headers.Count > 0)
                {
                    return this.Headers[^1];
                }

                return string.Empty;
            }
        }
        public double EG { get => GetEntropyOfTable(); }

        private double GetEntropyOfTable() 
        {
            var distinctResultValues = this.GetUniqueColumnValues(ResultCategory).ToList();
            return Calculation.CalculateEntropy(distinctResultValues, ResultSetValues);
        }

        public CsvData Filter(string featureName, string distinctValue)
        {
            // collect row data for given distinctValue.
            var newRows = new List<string>();
            var columns = new Dictionary<string, List<string>>();

            // 1. collect headers where x != featurename
            // 2. collect all rows containing distinctValue
            // 3. create subset containing relevant headers with corresponing rows 
            //  rows should not contain distinctValue

            var headers = TakeColumnsExcludingCurrentFeature(featureName);

            // TODO: fix, if multiple columns (different) have the same value -> relevantRows would return incorrect data!
            var relevantRows = this.Rows.Where(x => x.Contains(distinctValue)).ToList();
            newRows.AddRange(from item in relevantRows
                             let current = item.Replace($"{distinctValue};", string.Empty)
                             select current.Trim('\r', '\n'));

            foreach (var header in headers)
            {
                var headerIndex = headers.IndexOf(header);
                var columnValues = GetColumnValues(newRows, headerIndex);
                columns.Add(header, columnValues);
            }

            var dataSubSet = new CsvData
            {
                Columns = columns,
                Rows = newRows,
                Headers = headers
            };

            return dataSubSet;
        }

        private List<string> GetResultSetValues()
        {
            return this.Columns[ResultCategory];
        }

        private List<string> TakeColumnsExcludingCurrentFeature(string feature)
        {
            var tmpHeaders = new List<string>();
            tmpHeaders.AddRange(this.Headers);

            tmpHeaders.Remove(feature);
            return tmpHeaders;
        }

        private List<string> TakeRelevantRowsFromCurrentFeature(string feature)
        {
            return null;
        }

        private List<string> GetColumnValues(List<string> rows, int headerIndex)
        {
            // when we are looking at the resultcategory index we are getting IndexOutOfRange
            // if we split into subsets, since our data is getting smaller.
            
            // hack
            var index = this.Headers[headerIndex] == this.ResultCategory ? headerIndex - 1 : headerIndex;
            return rows.Select(x =>
            {

                var split = x.Split(FormValidator.ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries);
                var selected = split[index];
                return selected;
            }).ToList();
        }
    }
}
