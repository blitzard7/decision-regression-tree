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
                "Sunny;Yes;",
                "Windy;No;",
                "Sunny;No;",
                "Sunny;No;",
                "Outlook;Yes",
                "Windy;No;"
            };
            columns.Add(outlook, outlookData);
            columns.Add(needUmbrella, needUmbrellaData);
            csvData.Columns = columns;
            csvData.Rows.AddRange(rows);
            csvData.Headers.AddRange(new List<string>() { outlook, needUmbrella });

            // Act
            var result = decisionTree.BuildTree(csvData);

            // Assert
            Assert.Null(result);
        }
    }
}
