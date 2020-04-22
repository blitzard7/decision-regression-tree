using DecisionTree.Logic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DecisionTree.Logic.Services
{
    public class CsvReader : ICsvReader
    {
        public string Read(string file)
        {
            var data = File.ReadAllLines(file);

            if (data.Length <= 0)
            {
                return string.Empty;
            }

            var columNames = GetData(data[0]);
            return "";
        }

        public CsvData CreateDataTableFromCsvFile(string[] data)
        {
            var csvData = new CsvData();
            var columns = GetColumns(data);
            var rows = GetRows(data[1..^1]);


            return csvData;
        }

        private List<CsvRow> GetRows(string[] data)
        {
            throw new NotImplementedException();
        }

        private List<CsvColumn> GetColumns(string[] data)
        {
            var columnNames = GetData(data[0]);
            var columns = GetColumnsValues(columnNames, data[1..^1]);

            return columns;
        }

        private List<CsvColumn> GetColumnsValues(string[] columnNames, string[] data)
        {
            var column = new List<CsvColumn>();

            for (int i = 0; i < columnNames.Length; i++)
            {
                var current = columnNames[i];
                var csvColumn = new CsvColumn
                {
                    Name = current
                };

                for (int j = 0; j < data.Length; j++)
                {
                    var columnValue = GetData(data[j])[i];
                    csvColumn.Values.Add(columnValue);
                }
            }

            return column;
        }

        public string[] GetData(string file)
        {
            const string Splitter = ";";
            var columns = file.Split(Splitter, System.StringSplitOptions.RemoveEmptyEntries);
            return columns;
        }
    }
}
