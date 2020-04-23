using DecisionTree.Logic.Fluent;
using DecisionTree.Logic.Models;
using DecisionTree.Logic.Services;
using System.Collections.Generic;
using Xunit;

namespace DecisionTree.Logic.Tests
{
    public class CsvReaderTest
    {
        [Theory]
        [InlineData("Outlook;Temperatur;Humidity;Wind;NeedUmbrella;Next;Test", 7)]
        [InlineData("Outlook;Temperatur;Humidity;Wind;NeedUmbrella;", 5)]
        [InlineData("Outlook;Temperatur;Humidity;Wind;", 4)]
        [InlineData("", 0)]
        public void GetData_ShouldReturnColumnEntriesSplitBySemiColon(string data, int expected)
        {
            // Arrange
            var csvReader = new CsvReader();

            // Act 
            var columns = csvReader.GetData(data);

            // Assert
            Assert.Equal(expected, columns.Length);
        }

        [Fact]
        public void CreateDataTableFromCsvFile_ShouldReturnFilledDataTable()
        {
            // Arrange
            var csvReader = new CsvReader();

            // Act 
            var dataTable = csvReader.CreateDataTableFromCsvFile(TestData.TestCsvContent);

            // Assert
            Assert.True(dataTable.Columns.Count != 0 && dataTable.Values.Count != 0);
        }

        [Fact]
        public void SplitTable_ShouldReturnEmptyEntriesWhenNoFileWasGiven()
        {
            // Arrange
            var csvReader = new CsvReader();

            // Act 
            var tableData = csvReader.SplitTable(string.Empty);

            // Assert
            Assert.Empty(tableData);
        }

        [Theory]
        [InlineData("Outlook;Temperatur;Humidity;Wind;NeedUmbrella;\r\nSunny;Hot;High;Weak;Yes;\r\nSunny;Hot;High;Strong;Yes;\r\nOvercast;Hot;High;Weak;No;", 4)]
        [InlineData("Outlook;Temperatur;Humidity;Wind;NeedUmbrella;\r\n", 1)]
        public void SplitTable_ShouldReturnTableEntriesWhenFileIsGiven(string data, int expectedLength)
        {
            // Arrange
            var csvReader = new CsvReader();

            // Act 
            var tableData = csvReader.SplitTable(data);

            // Assert
            Assert.Equal(expectedLength, tableData.Length);
        }

        private List<CsvColumn> CreateCsvColumnsData()
        {
            var columns = new List<CsvColumn>();

            var colOutlook = new CsvColumnBuilder()
                .WithColumnName("Outlook")
                .WithColumnEntries(new List<string>() { "Sunny", "Sunny" })
                .Build();

            var colTemperatur = new CsvColumnBuilder()
                .WithColumnName("Temperatur")
                .WithColumnEntries(new List<string>() { "Hot", "Hot" })
                .Build();

            var colHumidity = new CsvColumnBuilder()
                .WithColumnName("Humidity")
                .WithColumnEntries(new List<string>() { "High", "High" })
                .Build();

            var colWind = new CsvColumnBuilder()
                .WithColumnName("Wind")
                .WithColumnEntries(new List<string>() { "Weak", "Strong" })
                .Build();

            var colNeedUmbrella = new CsvColumnBuilder()
                .WithColumnName("NeedUmbrella")
                .WithColumnEntries(new List<string>() { "Yes", "Yes" })
                .Build();

            columns.AddRange(new List<CsvColumn>(){ colOutlook, colTemperatur, colHumidity, colWind, colNeedUmbrella });

            return columns;
        }
    }
}
