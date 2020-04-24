using System;
using System.Linq;

namespace DecisionTree.Logic.Validator
{
    public class FormValidator : IValidator
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

            if (splitColumn.Length <= 0 || splitColumn[0].Equals(input))
            {
                return false;
            }

            return true;
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

        public bool IsHeaderPresent(string input)
        {
            var tmpInput = input.Replace(ValidValuesEntrySeparator, string.Empty);
            if (tmpInput.StartsWith(ValidDataSeparator))
            {
                return false;
            }

            var data = tmpInput.Split(ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length <= 0 || data[0].Equals(tmpInput))
            {
                return false;
            }

            return true;
        }

        public bool IsMetaInformationFormatValid(string input)
        {
            if (input.Length <= 0 || ! (IsHeaderPresent(input) && IsDataPresent(input)))
            {
                return false;
            }

            return true;
        }

        public bool IsRowFormatValid(string[] rows)
        {
            if (rows.Length <= 0)
            {
                return false;
            }

            return rows.All(x =>
            {
                var data = x.Split(ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries);
                if (data.Length <= 0 || data[0].Equals(x))
                {
                    return false;
                }

                return true;
            });
        }
    }
}
