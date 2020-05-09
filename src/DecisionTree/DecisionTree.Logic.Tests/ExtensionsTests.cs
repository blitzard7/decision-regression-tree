using DecisionTree.Logic.Models;
using System.Collections.Generic;
using Xunit;

namespace DecisionTree.Logic.Tests
{
    public class ExtensionsTests
    {
        [Fact]
        public void GetUniqueColumnValues_ShouldReturnUniqueValuesFromGivenKey()
        {
            // Arrange
            var csvData = new CsvData();
            var columns = new Dictionary<string, List<string>>();
            var key = "Outlook";
            var columnData = new List<string>
            {
                "Sunny",
                "Windy",
                "Sunny",
                "Sunny",
                "Outlook",
                "Windy"
            };

            columns.Add(key, columnData);
            csvData.Columns = columns;

            // Act
            var distinctValues = csvData.GetUniqueColumnValues(key);

            // Assert
            Assert.NotEqual(columnData, distinctValues);
        }

        [Fact]
        public void GetUniqueColumnValues_ShouldThrowKeyNotFoundExceptionWhenSearchKeyIsNotGivenInDictionary()
        {
            // Arrange
            var csvData = new CsvData();
            var columns = new Dictionary<string, List<string>>();
            var key = "Outlook";
            var columnData = new List<string>
            {
                "Sunny",
                "Windy",
                "Sunny",
                "Sunny",
                "Outlook",
                "Windy"
            };

            columns.Add(key, columnData);
            csvData.Columns = columns;

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() =>
            {
                csvData.GetUniqueColumnValues("Notgiven");
            });
        }

        [Fact]
        public void GetSpecificRowEntries_ShouldReturnRowEntriesWithRequestedSearchString()
        {
            // Arrange
            var csvData = new CsvData();
            var columns = new Dictionary<string, List<string>>();
            var outlook = "Outlook";
            var outlookData = new List<string>
            {
                "Sunny",
                "Windy",
                "Sunny",
                "Sunny",
                "Outlook",
                "Windy"
            };
            var temperatur = "Temperatur";
            var temperaturData = new List<string>
            {
                "Hot",
                "Cold",
                "Mild",
                "Hot",
                "Mild",
                "Cold"
            };
            var rows = new List<string>()
            {
                "Sunny;Hot;",
                "Windy;Cold;",
                "Sunny;Mild;",
                "Sunny;Hot;",
                "Outlook;Mild;",
                "Windy;Cold;"
            };
            var expected = new List<string>()
            {
                "Windy;Cold;",
                "Windy;Cold;"
            };

            columns.Add(outlook, outlookData);
            columns.Add(temperatur, temperaturData);
            csvData.Columns = columns;
            csvData.Rows.AddRange(rows);

            // Act 
            var actual = csvData.GetSpecificRowEntries("Cold");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetSpecificRowEntries_ShouldReturnEmptyCollectionWhenNoDataFound()
        {
            // Arrange
            var csvData = new CsvData();
            var columns = new Dictionary<string, List<string>>();
            var outlook = "Outlook";
            var outlookData = new List<string>
            {
                "Sunny",
                "Windy",
                "Sunny",
                "Sunny",
                "Outlook",
                "Windy"
            };
            var temperatur = "Temperatur";
            var temperaturData = new List<string>
            {
                "Hot",
                "Cold",
                "Mild",
                "Hot",
                "Mild",
                "Cold"
            };
            var rows = new List<string>()
            {
                "Sunny;Hot;",
                "Windy;Cold;",
                "Sunny;Mild;",
                "Sunny;Hot;",
                "Outlook;Mild;",
                "Windy;Cold;"
            };

            columns.Add(outlook, outlookData);
            columns.Add(temperatur, temperaturData);
            csvData.Columns = columns;
            csvData.Rows.AddRange(rows);

            // Act 
            var actual = csvData.GetSpecificRowEntries("No");

            // Assert
            Assert.Empty(actual);
        }
    }
}
