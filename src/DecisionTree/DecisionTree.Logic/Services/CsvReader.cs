using DecisionTree.Logic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DecisionTree.Logic.Services
{
    public class CsvReader : ICsvReader
    {
        public string[] Read(string file)
        {
            var data = File.ReadAllLines(file);

            if (data.Length <= 0)
            {
                return Array.Empty<string>();
            }

            return data;
        }

        public CsvData CreateDataTableFromCsvFile(string rawData)
        {
            var data = SplitTable(rawData);
            var columnNames = GetData(data[0]);
            var columns = GetColumns(columnNames, data);
            var rows = GetRows(data[1..^0]);

            var csvData = new CsvData();
            csvData.Columns.AddRange(columns);
            csvData.Values.AddRange(rows);

            return csvData;
        }

        public string[] SplitTable(string file)
        {
            const string Splitter = "\r\n";
            var data = file.Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
            return data;
        }

        public string[]  GetData(string file)
        {
            const string Splitter = ";";
            var data = file.Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
            data = data.Select(x => x.Trim()).ToArray();
            return data;
        }

        private IEnumerable<CsvRow> GetRows(IEnumerable<string> data)
        {
            var rows = new List<CsvRow>();
            
            foreach (var item in data)
            {
                var rowData = GetData(item);
                var currentRow = new CsvRow();
                currentRow.Values.AddRange(rowData);
                rows.Add(currentRow);
            }

            return rows;
        }

        private IEnumerable<CsvColumn> GetColumns(string[] columnNames, string[] data)
        {
            return GetColumnsValues(columnNames, data[1..^0]);
        }

        private IEnumerable<CsvColumn> GetColumnsValues(string[] columnNames, string[] data)
        {
            var columns = new List<CsvColumn>();

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

                columns.Add(csvColumn);
            }

            return columns;
        }
    }
}
