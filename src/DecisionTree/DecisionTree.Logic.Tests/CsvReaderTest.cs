using DecisionTree.Logic.Services;
using System;
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
        public void CsvReaderTest_GetData_ShouldReturnColumnEntriesSplitBySemiColon(string data, int expected)
        {
            // Arrange
            var csvReader = new CsvReader();

            // Act 
            var columns = csvReader.GetData(data);

            // Assert
            Assert.Equal(expected, columns.Length);
        }
    }
}
