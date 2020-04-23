using DecisionTree.Logic.Models;
using System.Collections.Generic;

namespace DecisionTree.Logic.Fluent
{
    public class CsvColumnBuilder
    {
        private CsvColumn _csvColumn;

        public CsvColumnBuilder()
        {
            _csvColumn = new CsvColumn();
        }

        public CsvColumnBuilder WithColumnName(string name)
        {
            _csvColumn.Name = name;
            return this;
        }

        public CsvColumnBuilder WithColumnEntry(string value)
        {
            _csvColumn.Values.Add(value);
            return this;
        }

        public CsvColumnBuilder WithColumnEntries(IEnumerable<string> values)
        {
            _csvColumn.Values.AddRange(values);
            return this;
        }

        public void Clear()
        {
            _csvColumn = new CsvColumn();
        }

        public CsvColumn Build()
        {
            var csvColumn = _csvColumn;
            Clear();
            return csvColumn;
        }
    }
}
