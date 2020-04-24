using DecisionTree.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DecisionTree.Logic.Tests
{
    public class CsvServiceTest
    {
        [Fact]
        public void IsImportedFileValid_ShouldReturnFalseIfEmptyFileHasBeenSelected()
        {
            // Arrange 
            var csvService = new CsvService();
            var file = string.Empty;

            // Act 
            var isFileValid = csvService.IsImportedFileValid(file);

            // Assert
            Assert.False(isFileValid);
        }

        [Fact]
        public void IsImportedFileValid_ShouldReturnFalseIfFileStartswithDataTag()
        {
            // Arrange 
            var csvService = new CsvService();
            var file = @"Data
Sunny;Hot;High;Weak;Yes;";

            // Act 
            var isFileValid = csvService.IsImportedFileValid(file);

            // Assert
            Assert.False(isFileValid);
        }

        [Fact]
        public void IsImportedFileValid_ShouldReturnFalseIfFileDoesNotContainDataTag()
        {
            // Arrange 
            var csvService = new CsvService();
            var file = @"Outlook;Temperatur;Humidity;Wind;NeedUmbrella;
Sunny;Hot;High;Weak;Yes;";

            // Act 
            var isFileValid = csvService.IsImportedFileValid(file);

            // Assert
            Assert.False(isFileValid);
        }

        [Fact]
        public void IsImportedFileValid_ShouldReturnTrueIfFileContainsValidMetadata()
        {
            // Arrange 
            var csvService = new CsvService();
            var file = @"Outlook;Temperatur;Humidity;Wind;NeedUmbrella;
Data
Sunny;Hot;High;Weak;Yes;";

            // Act 
            var isFileValid = csvService.IsImportedFileValid(file);

            // Assert
            Assert.True(isFileValid);
        }

        [Fact]
        public void GetHeaderInformation_ShouldReturnListContainingHeaderInformation()
        {
            // Arrange
            var csvService = new CsvService();
            var expected = new List<string>()
            {
                "Outlook",
                "Temperatur",
                "Humidity",
                "Wind",
                "NeedUmbrella"
            };

            var file = @"Outlook;Temperatur;Humidity;Wind;NeedUmbrella;";

            // Act 
            var header = csvService.GetHeaderInformation(file).ToList();

            // Assert
            Assert.Equal(expected, header);
        }

        [Fact]
        public void GetHeaderInformation_ShouldReturnEmptyListWhenNoHeaderGiven()
        {
            // Arrange
            var csvService = new CsvService();

            // Act 
            var header = csvService.GetHeaderInformation(string.Empty);

            // Assert
            Assert.Empty(header);
        }

        [Theory]
        [InlineData("")]
        [InlineData("123456")]
        [InlineData("asdf")]
        [InlineData("row1,row2,row3,row4")]
        public void GetDataInformation_ShouldReturnEmptyListWhenNoDataGiven(string input)
        {
            // Arrange
            var csvService = new CsvService();

            // Act 
            var data = csvService.GetDataInformation(input);

            // Assert
            Assert.Empty(data);
        }

        [Fact]
        public void GetDataInformation_ShouldReturnSplitDataInformation()
        {
            // Arrange
            var csvService = new CsvService();
            var expected = new List<string>()
            {
                "Sunny",
                "Hot",
                "High",
                "Weak",
                "Yes"
            };

            var input = "Sunny;Hot;High;Weak;Yes;";

            // Act 
            var data = csvService.GetDataInformation(input);

            // Assert
            Assert.Equal(expected, data);
        }

        [Fact]
        public void CreateColumns_ShouldThrowIndexOutOfRangeExceptionWhenEmptyMetaInformationIsGiven()
        {
            // Arrange
            var csvService = new CsvService();
            string[] metaInformation = new string[0];

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                csvService.CreateColumns(metaInformation);
            });
        }

        [Fact]
        public void CreateColumns_ShouldThrowIndexOutOfRangeExceptionWhenOneElementyMetaInformationIsGiven()
        {
            // Arrange
            var csvService = new CsvService();
            string[] metaInformation = new string[1] { "test" };

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                csvService.CreateColumns(metaInformation);
            });
        }

        [Fact]
        public void CreateColumns_ShouldThrowNullReferenceExceptionWhenNoMetaInformationIsGiven()
        {
            // Arrange
            var csvService = new CsvService();
            string[] metaInformation = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                csvService.CreateColumns(metaInformation);
            });
        }

        [Fact]
        public void CreateColumns_ShouldReturnEmptyDictionaryWhenMetaInformationIsInvalid()
        {
            // Arrange
            var csvService = new CsvService();
            string[] metaInformation = new string[2] { "asdf,asdf,asdf", "1,2,3,4" };

            // Act 
            var columns = csvService.CreateColumns(metaInformation);

            // Assert
            Assert.Empty(columns);
        }

        [Fact]
        public void CreateColumns_ShouldReturnEmptyValuesInDictionaryWhenDataInformationIsInvalid()
        {
            // Arrange
            var csvService = new CsvService();
            string[] metaInformation = new string[2] { "col1;col2;col3", "1,2,3,4" };

            // Act 
            var columns = csvService.CreateColumns(metaInformation);

            // Assert
            Assert.Empty(columns.Values);
        }

        [Fact]
        public void CreateColumns_ShouldReturnNonEmptyDictionaryWhenMetaInformationIsValid()
        {
            // Arrange
            var csvService = new CsvService();
            string[] metaInformation = new string[2] { "col1;col2;col3", "1;2;3;4" };

            // Act 
            var columns = csvService.CreateColumns(metaInformation);

            // Assert
            Assert.NotEmpty(columns);
        }
    }
}
