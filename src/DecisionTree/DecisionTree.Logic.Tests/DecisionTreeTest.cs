using DecisionTree.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DecisionTree.Logic.Tests
{
    public class DecisionTreeTest
    {
        [Fact]
        public void BuildTree_ShouldReturnNull()
        {
            // Arrange
            var decisionTree = new Trees.DecisionTree();
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
            var humidity = "Humidity";
            var humidityData = new List<string>
            {
                "high",
                "high",
                "normal",
                "high",
                "high",
                "normal"
            };
            var needUmbrella = "NeedUmbrella";
            var needUmbrellaData = new List<string>
            {
                "Yes",
                "No",
                "No",
                "No",
                "Yes",
                "No"
            };
            var rows = new List<string>()
            {
                "Sunny;high;Yes;",
                "Windy;high;No;",
                "Sunny;normal;No;",
                "Sunny;high;Yes;",
                "Outlook;high;Yes",
                "Windy;normal;No;"
            };
            columns.Add(outlook, outlookData);
            columns.Add(humidity, humidityData);
            columns.Add(needUmbrella, needUmbrellaData);
            csvData.Columns = columns;
            csvData.Rows.AddRange(rows);
            csvData.Headers.AddRange(new List<string>() { outlook, humidity, needUmbrella });

            // Act
            var result = decisionTree.BuildTree(csvData);

            // Assert
            Assert.Null(result);
        }
    }
}
