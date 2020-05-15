using DecisionTree.Logic.Interfaces;
using System;
using System.Linq;

namespace DecisionTree.Logic.Validator
{
    public class FormValidator : IFormValidator
    {
        public const string ValidValueSeparator = ";";
        public const string ValidDataSeparator = "Data";
        public const string ValidValuesEntrySeparator = "\r\n";

        public bool AreAmountOfRowValuesWithColumnEntriesEqual(string[] column, string[] rows)
        {
            return rows.All(row =>
            {
                var splitData = row.Split(ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries);
                return splitData.Length == column.Length;
            });
        }

        public bool IsColumnFormatValid(string input)
        {
            var splitColumn = input.Split(ValidDataSeparator, StringSplitOptions.RemoveEmptyEntries);

            return splitColumn.Length > 0 && !splitColumn[0].Equals(input);
        }

        public bool IsDataPresent(string input)
        {
            var splitMetaInformation = input.Split(ValidDataSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (splitMetaInformation.Length < 2)
            {
                return false;
            }

            var rows = splitMetaInformation[1].Split(ValidValuesEntrySeparator, StringSplitOptions.RemoveEmptyEntries);
            return IsRowFormatValid(rows);
        }

        public bool IsDataSeparatedCorrectly(string data)
        {
            var splitData = data.Split(ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries);

            if (splitData.Length == 0)
            {
                return false;
            }

            return !splitData.Contains(data) || splitData.Length != 1;
        }

        public bool IsHeaderPresent(string input)
        {
            var tmpInput = input.Replace(ValidValuesEntrySeparator, string.Empty);
            if (tmpInput.StartsWith(ValidDataSeparator))
            {
                return false;
            }

            var data = tmpInput.Split(ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries);
            return data.Length > 0 && !data[0].Equals(tmpInput);
        }

        public bool IsMetaInformationFormatValid(string input)
        {
            return input.Length > 0 && (IsHeaderPresent(input) && IsDataPresent(input));
        }

        public bool IsRowFormatValid(string[] rows)
        {
            return rows.Length > 0 && rows.All(IsRowFormatValid);
        }

        public bool IsRowFormatValid(string row)
        {
            var data = row.Split(ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries);

            // what happens when there is only 1 column and 1 row?
            return data.Length > 0 && !data[0].Equals(row);
        }
    }
}
