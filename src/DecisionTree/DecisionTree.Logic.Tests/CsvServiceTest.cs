﻿using DecisionTree.Logic.Interfaces;
using DecisionTree.Logic.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using DecisionTree.Logic.Exceptions;
using Xunit;

namespace DecisionTree.Logic.Tests
{
    public class CsvServiceTest
    {

        [Fact]
        public void GetHeaderInformation_ShouldReturnListContainingHeaderInformation()
        {
            // Arrange
            var formValidatorMock = new Mock<IFormValidator>();
            var csvService = new CsvService(formValidatorMock.Object);
            var expected = new List<string>()
            {
                "Outlook",
                "Temperature",
                "Humidity",
                "Wind",
                "NeedUmbrella"
            };

            var file = @"Outlook;Temperature;Humidity;Wind;NeedUmbrella;";

            // Act 
            var header = csvService.GetHeaderInformation(file).ToList();

            // Assert
            Assert.Equal(expected, header);
        }

        [Fact]
        public void GetHeaderInformation_ShouldReturnEmptyListWhenNoHeaderGiven()
        {
            // Arrange
            var formValidatorMock = new Mock<IFormValidator>();
            var csvService = new CsvService(formValidatorMock.Object);

            // Act 
            var header = csvService.GetHeaderInformation(string.Empty);

            // Assert
            Assert.Empty(header);
        }

        [Fact]
        public void GetDataInformation_ShouldReturnSplitDataInformation()
        {
            // Arrange
            var formValidatorMock = new Mock<IFormValidator>();
            var csvService = new CsvService(formValidatorMock.Object);
            var expected = new List<string>()
            {
                "sunny;hot;high;weak;yes;"
            };

            const string input = "Sunny;Hot;High;Weak;Yes;";

            // Act 
            var data = csvService.GetRowValues(input);

            // Assert
            Assert.Equal(expected, data);
        }

        [Fact]
        public void CreateColumns_ShouldThrowIndexOutOfRangeExceptionWhenEmptyMetaInformationIsGiven()
        {
            // Arrange
            var formValidatorMock = new Mock<IFormValidator>();
            formValidatorMock.Setup(x => x.IsRowFormatValid(It.IsAny<string[]>())).Returns(false);
            var csvService = new CsvService(formValidatorMock.Object);
            var metaInformation = new string[0];

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                csvService.CreateColumns(metaInformation);
            });
        }

        [Fact]
        public void CreateColumns_ShouldThrowIndexOutOfRangeExceptionWhenOneElementMetaInformationIsGiven()
        {
            // Arrange
            var formValidatorMock = new Mock<IFormValidator>();
            formValidatorMock.Setup(x => x.IsRowFormatValid(It.IsAny<string[]>())).Returns(false);
            var csvService = new CsvService(formValidatorMock.Object);
            var metaInformation = new string[1] { "test" };

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
            var formValidatorMock = new Mock<IFormValidator>();
            formValidatorMock.Setup(x => x.IsRowFormatValid(It.IsAny<string[]>())).Returns(false);
            var csvService = new CsvService(formValidatorMock.Object);
            string[] metaInformation = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                csvService.CreateColumns(metaInformation);
            });
        }

        [Fact]
        public void CreateColumns_ShouldThrowCsvRowFormatExceptionWhenMetaInformationIsInvalid()
        {
            // Arrange
            var formValidatorMock = new Mock<IFormValidator>();
            formValidatorMock.Setup(x => x.IsRowFormatValid(It.IsAny<string[]>())).Returns(false);
            var csvService = new CsvService(formValidatorMock.Object);
            var metaInformation = new string[2] { "asdf,asdf,asdf", "1,2,3,4" };

            // Act && Assert
            Assert.Throws<CsvRowFormatInvalidException>(() =>
            {
                csvService.CreateColumns(metaInformation);
            });
        }

        [Fact]
        public void CreateColumns_ShouldThrowCsvRowFormatExceptionWhenDataInformationIsInvalid()
        {
            // Arrange
            var formValidatorMock = new Mock<IFormValidator>();
            formValidatorMock.Setup(x => x.IsRowFormatValid(It.IsAny<string[]>())).Returns(false);
            var csvService = new CsvService(formValidatorMock.Object);
            var metaInformation = new string[2] { "col1;col2;col3", "1,2,3,4" };

            // Act && Assert
            Assert.Throws<CsvRowFormatInvalidException>(() =>
            {
                csvService.CreateColumns(metaInformation);
            });
        }

        [Fact]
        public void CreateColumns_ShouldReturnNonEmptyDictionaryWhenMetaInformationIsValid()
        {
            // Arrange
            var formValidatorMock = new Mock<IFormValidator>();
            formValidatorMock.Setup(x => x.IsRowFormatValid(It.IsAny<string[]>())).Returns(true);
            var csvService = new CsvService(formValidatorMock.Object);
            var metaInformation = new string[2] { "col1;col2;col3", "1;2;3;4" };

            // Act 
            var columns = csvService.CreateColumns(metaInformation);

            // Assert
            Assert.NotEmpty(columns);
            formValidatorMock.Verify(x => x.IsRowFormatValid(It.IsAny<string[]>()), Times.Once);
        }
    }
}
