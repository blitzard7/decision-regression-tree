using System.Collections.Generic;

namespace DecisionTree.Logic.Interfaces
{
    /// <summary>
    /// Represents the IFormValidator
    /// </summary>
    public interface IFormValidator
    {
        /// <summary>
        /// Checks whether the amount of columns and amount of row values are equal.
        /// </summary>
        /// <param name="column">The columns.</param>
        /// <param name="rows">The row values.</param>
        /// <returns>Returns a value indicating whether the amount of columns and row values are equal or not.</returns>
        bool AreRowValuesAlignedWithColumns(IEnumerable<string> column, IEnumerable<string> rows);

        /// <summary>
        /// Checks whether the column format of the given input is valid.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns a value whether the input contains a valid column format.</returns>
        bool IsColumnStructureValid(string input);

        /// <summary>
        /// Checks whether the Data tag is present in the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns a value indicating whether the input contains the Data tag.</returns>
        bool ContainsDataTag(string input);

        /// <summary>
        /// Checks whether the row data is separated correctly. 
        /// </summary>
        /// <param name="rowData">The row data.</param>
        /// <returns>Returns a value whether the row data is separated correctly or not.</returns>
        bool IsRowDataDelimiterValid(string rowData);

        /// <summary>
        /// Checks whether or not the header is present.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns a value indicating whether the input contains header or not.</returns>
        bool ContainsHeader(string input);

        /// <summary>
        /// Checks whether the meta information of the input is valid.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns a value indicating whether the input contains valid meta information or not.</returns>
        bool IsMetaInformationValid(string input);

        /// <summary>
        /// Checks whether the rows format is valid.
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <returns>Returns a value whether the rows have a valid format or not.</returns>
        bool IsRowFormatValid(string[] rows);

        /// <summary>
        /// Checks whether the rows format is valid.
        /// </summary>
        /// <param name="row">The rows.</param>
        /// <returns>Returns a value whether the rows have a valid format or not.</returns>
        bool IsRowFormatValid(string row);
    }
}
