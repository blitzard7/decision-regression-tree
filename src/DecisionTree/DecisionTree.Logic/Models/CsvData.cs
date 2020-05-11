using DecisionTree.Logic.Helper;
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

        public CsvData Fitter(string featureName, string distinctValue)
        {
            // collect row data for given distinctValue.
            var headers = new List<string>();
            var newRows = new List<string>();
            var columns = new Dictionary<string, List<string>>();

            // 1. collect headers where x != featurename
            // 2. collect all rows containing distinctValue
            // 3. create subset containing relevant headers with corresponing rows 
            //  rows should not contain distinctValue
            
            headers.AddRange(this.Headers.SkipWhile(x => x == featureName));
            var relevantRows = this.Rows.Where(x => x.ToLower().Contains(distinctValue)).ToList();
            newRows.AddRange(from item in relevantRows
                             let current = item.ToLower().Replace($"{distinctValue};", string.Empty)
                             select current);

            foreach (var header in headers)
            {
                columns.Add(header, this.Columns[header]);
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
    }
}
