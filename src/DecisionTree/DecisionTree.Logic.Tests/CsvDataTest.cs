using DecisionTree.Logic.Models;
using System.Collections.Generic;
using Xunit;

namespace DecisionTree.Logic.Tests
{
    public class CsvDataTest
    {
        [Fact]
        public void Fitter_ShouldReturnSubSequenceWithoutRequestedHeader()
        {
            // Arrange
            var csvData = new CsvData();
            var columns = new Dictionary<string, List<string>>();
            const string outlook = "Outlook";
            var outlookData = new List<string>
            {
                "Sunny",
                "Windy",
                "Sunny",
                "Sunny",
                "Outlook",
                "Windy"
            };
            const string humidity = "Humidity";
            var humidityData = new List<string>
            {
                "high",
                "high",
                "normal",
                "high",
                "high",
                "normal"
            };
            const string needUmbrella = "NeedUmbrella";
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
            var fitter = csvData.Filter(outlook, "sunny");

            // Assert
            Assert.DoesNotContain("sunny", fitter.Rows);
        }
    }
}
