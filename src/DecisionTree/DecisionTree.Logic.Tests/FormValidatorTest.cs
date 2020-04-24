
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
            var isDataPresent = formValidator.IsDataPresent(input);

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
            var isDataPresent = formValidator.IsDataPresent(input);

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
    }
}
