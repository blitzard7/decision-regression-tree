using DecisionTree.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionTree.Logic.Fluent
{
    public class CsvRowBuilder
    {
        private CsvRow _csvRow;

        public CsvRowBuilder()
        {
            _csvRow = new CsvRow();
        }

        public void Clear()
        {
            _csvRow = new CsvRow();
        }

        public CsvRowBuilder WithValue(string value)
        {
            _csvRow.Values.Add(value);
            return this;
        }

        public CsvRowBuilder WithValues(IEnumerable<string> values)
        {
            _csvRow.Values.AddRange(values);
            return this;
        }

        public CsvRow Build()
        {
            var csvRow = _csvRow;
            Clear();
            return csvRow;
        }
    }
}
