
using DecisionTree.Logic.Validator;
using Xunit;

namespace DecisionTree.Logic.Tests
{
    public class FormValidatorTest
    {
        [Fact]
        public void IsDataPresent_ShouldReturnFalseWhenMetaInformationDoesNotContainDataTag()
        {
            // Arrange 
            var formValidator = new FormValidator();
            var input = TestData.TestCsvInvalidContentDataMissing;

            // Act 
            var isDataPresent = formValidator.IsDataTagPresent(input);

            // Assert
            Assert.False(isDataPresent);
        }

        [Fact]
        public void IsDataPresent_ShouldReturnTrueWhenMetaInformationDoesNotContainDataTag()
        {
            // Arrange 
            var formValidator = new FormValidator();
            var input = TestData.TestCsvContent;

            // Act 
            var isDataPresent = formValidator.IsDataTagPresent(input);

            // Assert
            Assert.True(isDataPresent);
        }

        [Theory]
        [InlineData("")]
        [InlineData("\r\nData\r\n1;2;3;4;5")]
        [InlineData("col1,col2,col3")]
        public void IsHeaderPresent_ShouldReturnFalseWhenMetaInformationDoesNotContainHeader(string input)
        {
            // Arrange
            var formValidator = new FormValidator();

            // Act
            var isHeaderPresent = formValidator.IsHeaderPresent(input);

            // Assert
            Assert.False(isHeaderPresent);
        }

        [Theory]
        [InlineData("col1;col2;col3")]
        [InlineData("col4;col5;")]
        public void IsHeaderPresent_ShouldReturnTrueWhenMetaInformationDoesContainHeader(string input)
        {
            // Arrange
            var formValidator = new FormValidator();

            // Act
            var isHeaderPresent = formValidator.IsHeaderPresent(input);

            // Assert
            Assert.True(isHeaderPresent);
        }

        [Theory]
        [InlineData("")]
        [InlineData("col1,col2\r\nData\r\n")]
        [InlineData("col1;col2;\r\nData\r\n")]
        public void IsMetaDataFormatValid_ShouldReturnFalseWhenMetaInformationIsInvalid(string input)
        {
            // Arrange
            var formValidator = new FormValidator();

            // Act
            var isMetaInformationValid = formValidator.IsMetaInformationFormatValid(input);

            // Assert
            Assert.False(isMetaInformationValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("data1,data2,data3", "data4,data5")]
        [InlineData("asdf")]
        public void IsRowFormatValid_ShouldReturnFalseWhenRowFormatIsInvalid(params string[] rows)
        {
            // Arrange 
            var formValidator = new FormValidator();

            // Act
            var isRowFormatValid = formValidator.IsRowFormatValid(rows);

            // Assert
            Assert.False(isRowFormatValid);
        }

        [Theory]
        [InlineData("data1;data2")]
        [InlineData("data1;data2;data3", "data4;data5")]
        public void IsRowFormatValid_ShouldReturnTrueWhenRowFormatIsValid(params string[] rows)
        {
            // Arrange 
            var formValidator = new FormValidator();

            // Act
            var isRowFormatValid = formValidator.IsRowFormatValid(rows);

            // Assert
            Assert.True(isRowFormatValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("col1,col2")]
        [InlineData("this is not a column")]
        public void IsColumnFormatValid_ShouldReturnFalseWhenColumnFormatIsInvalid(string columns)
        {
            // Arrange 
            var formValidator = new FormValidator();

            // Act
            var isColumnFormatValid = formValidator.IsColumnFormatValid(columns);

            // Assert
            Assert.False(isColumnFormatValid);
        }

        [Fact]
        public void IsColumnFormatValid_ShouldReturnTrueWhenColumnFormatIsValid()
        {
            // Arrange 
            var formValidator = new FormValidator();
            var columns = "col1;col2;col3;col4";

            // Act
            var isColumnFormatValid = formValidator.IsColumnFormatValid(columns);

            // Assert
            Assert.False(isColumnFormatValid);
        }

        [Fact]
        public void AreAmountOfRowValuesWithColumnEntriesEqual_ShouldReturnFalseWhenRowAmountMismatchesColAmount()
        {
            // Arrange
            var formValidator = new FormValidator();
            var columns = new string[4]
            {
                "col1",
                "col2",
                "col3",
                "col4"
            };

            var rows = new string[4]
            {
                "data11,data12,data13",
                "data21,data22,data23",
                "data31,data32,data33",
                "data41,data42,data43"
            };

            // Act
            var amountSame = formValidator.AreAmountOfRowValuesWithColumnEntriesEqual(columns, rows);

            // Assert
            Assert.False(amountSame);
        }

        [Fact]
        public void AreAmountOfRowValuesWithColumnEntriesEqual_ShouldReturnTrueWhenRowAmountMatchesColAmount()
        {
            // Arrange
            var formValidator = new FormValidator();
            var columns = new string[4]
            {
                "col1",
                "col2",
                "col3",
                "col4"
            };

            var rows = new string[4]
            {
                "data11;data12;data13;data14",
                "data21;data22;data23;data24",
                "data31;data32;data33;data34",
                "data41;data42;data43;data44"
            };

            // Act
            var amountSame = formValidator.AreAmountOfRowValuesWithColumnEntriesEqual(columns, rows);

            // Assert
            Assert.True(amountSame);
        }

        [Theory]
        [InlineData("data,data,data,data")]
        [InlineData("")]
        [InlineData("I'm not valid")]
        public void IsDataSeparatedCorrectly_ShouldReturnFalseWhenDataIsNotSplitCorrectly(string data)
        {
            // Arrange 
            var formValidator = new FormValidator();

            // Act 
            var isDataSeparatedCorrectly = formValidator.IsRowDataSeparatedCorrectly(data);

            // Assert
            Assert.False(isDataSeparatedCorrectly);
        }

        [Theory]
        [InlineData("data;data;data;data")]
        [InlineData("I'm valid;because;I'm split, correctly")]
        public void IsDataSeparatedCorrectly_ShouldReturnTrueWhenDataIsSplitCorrectly(string data)
        {
            // Arrange 
            var formValidator = new FormValidator();

            // Act 
            var isDataSeparatedCorrectly = formValidator.IsRowDataSeparatedCorrectly(data);

            // Assert
            Assert.True(isDataSeparatedCorrectly);
        }
    }
}
