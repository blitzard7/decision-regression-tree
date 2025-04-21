using DecisionTree.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Logic.Validator
{
    public class FormValidator : IFormValidator
    {
        public const string ValidValueSeparator = ";";
        public const string ValidDataSeparator = "Data";
        public const string ValidValuesEntrySeparator = "\r\n";

        /// <summary>
        /// Checks whether the amount of columns and amount of row values are equal.
        /// </summary>
        /// <param name="column">The columns.</param>
        /// <param name="rows">The row values.</param>
        /// <returns>Returns a value indicating whether the amount of columns and row values are equal or not.</returns>
        public bool AreRowValuesAlignedWithColumns(IEnumerable<string> column, IEnumerable<string> rows)
        {
            var tmpCols = column.ToArray();
            var tmpRows = rows.ToArray();
            return tmpRows.All(row =>
            {
                var splitData = row.Split(ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries);
                return splitData.Length == tmpCols.Length;
            });
        }

        /// <summary>
        /// Checks whether the column format of the given input is valid.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns a value whether the input contains a valid column format.</returns>
        public bool IsColumnStructureValid(string input)
        {
            var splitColumn = input.Split(ValidDataSeparator, StringSplitOptions.RemoveEmptyEntries);

            return splitColumn.Length > 0 && !splitColumn[0].Equals(input);
        }

        /// <summary>
        /// Checks whether the Data tag is present in the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns a value indicating whether the input contains the Data tag.</returns>
        public bool ContainsDataTag(string input)
        {
            var splitMetaInformation = input.Split(ValidDataSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (splitMetaInformation.Length < 2)
            {
                return false;
            }

            var rows = splitMetaInformation[1].Split(ValidValuesEntrySeparator, StringSplitOptions.RemoveEmptyEntries);
            return IsRowFormatValid(rows);
        }

        /// <summary>
        /// Checks whether the row data is separated correctly. 
        /// </summary>
        /// <param name="rowData">The row data.</param>
        /// <returns>Returns a value whether the row data is separated correctly or not.</returns>
        public bool IsRowDataDelimiterValid(string rowData)
        {
            var splitData = rowData.Split(ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (splitData.Length == 0)
            {
                return false;
            }

            return !splitData.Contains(rowData) || splitData.Length != 1;
        }

        /// <summary>
        /// Checks whether or not the header is present.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns a value indicating whether the input contains header or not.</returns>
        public bool ContainsHeader(string input)
        {
            var tmpInput = input.Replace(ValidValuesEntrySeparator, string.Empty);
            if (tmpInput.StartsWith(ValidDataSeparator))
            {
                return false;
            }

            var data = tmpInput.Split(ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries);
            return data.Length > 0 && !data[0].Equals(tmpInput);
        }

        /// <summary>
        /// Checks whether the meta information of the input is valid.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Returns a value indicating whether the input contains valid meta information or not.</returns>
        public bool IsMetaInformationValid(string input)
        {
            return input.Length > 0 && (ContainsHeader(input) && ContainsDataTag(input));
        }

        /// <summary>
        /// Checks whether the rows format is valid.
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <returns>Returns a value whether the rows have a valid format or not.</returns>
        public bool IsRowFormatValid(string[] rows)
        {
            return rows.Length > 0 && rows.All(IsRowFormatValid);
        }

        /// <summary>
        /// Checks whether the rows format is valid.
        /// </summary>
        /// <param name="row">The rows.</param>
        /// <returns>Returns a value whether the rows have a valid format or not.</returns>
        public bool IsRowFormatValid(string row)
        {
            var data = row.Split(ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries);
            return data.Length > 0 && !data[0].Equals(row);
        }
    }
}
