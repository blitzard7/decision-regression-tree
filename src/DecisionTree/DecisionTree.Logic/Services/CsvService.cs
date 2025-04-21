using DecisionTree.Logic.Exceptions;
using DecisionTree.Logic.Interfaces;
using DecisionTree.Logic.Models;
using DecisionTree.Logic.Validator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Logic.Services
{
    /// <summary>
    /// Represents the CsvService class.
    /// </summary>
    public class CsvService : ICsvService
    {
        /// <summary>
        /// The form validator.
        /// </summary>
        private readonly IFormValidator _formValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvService"/> class.
        /// </summary>
        /// <param name="formValidator">The form validator.</param>
        public CsvService(IFormValidator formValidator)
        {
            _formValidator = formValidator ?? throw new ArgumentNullException(nameof(formValidator));
        }

        /// <summary>
        /// Creates the csv data structure from the imported file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <exception cref="CsvDataInvalidMetadataException">Is thrown when file does not match expected format.</exception>
        /// <returns>Returns the constructed <see cref="CsvData"/> from the imported file.</returns>
        public CsvData CreateCsvDataFromFile(string file)
        {
            if (!_formValidator.IsMetaInformationValid(file))
            {
                throw new CsvDataInvalidMetadataException($"Imported file does not match with expected format.");
            }

            var metaDataInformation = SplitMetaDataInformationFromRawFileContent(file).ToArray();
            var headerInformation = GetHeaderInformation(metaDataInformation[0]).ToList();
            var columns = CreateColumns(metaDataInformation);
            var rows = GetRowValues(metaDataInformation[1]).ToList();

            var areAmountOfRowsAndColsEqual = _formValidator.AreRowValuesAlignedWithColumns(headerInformation, rows);
            if (!areAmountOfRowsAndColsEqual)
            {
                throw new CsvDataInvalidMetadataException("Amount of column and row values do not match.");
            }

            var csvData = new CsvData
            {
                Headers = headerInformation,
                Columns = columns,
                Rows = rows
            };

            return csvData;
        }

        /// <summary>
        /// Get the header information of the imported file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Returns the name of the columns.</returns>
        public IEnumerable<string> GetHeaderInformation(string file)
        {
            var header = file.Split(FormValidator.ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (header.Contains(file))
            {
                return Array.Empty<string>();
            }

            if (header.Contains("\r\n"))
            {
                header.Remove(FormValidator.ValidValuesEntrySeparator);
            }

            var columns = header.Select(x => x.Trim('\r', '\n'));

            return columns;
        }

        /// <summary>
        /// Get the row information of the imported file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Returns the rows.</returns>
        public IEnumerable<string> GetRowValues(string file)
        {
            var data = file.Split(FormValidator.ValidValuesEntrySeparator, StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower());
            return data;
        }

        /// <summary>
        /// Creates the columns containing the column name as key and its corresponding row values.
        /// </summary>
        /// <param name="metaDataInformation">The metaDataInformation.</param>
        /// <exception cref="CsvRowFormatInvalidException">Is thrown when rows do not match expected format</exception>
        /// <returns>Returns columns.</returns>
        public Dictionary<string, List<string>> CreateColumns(string[] metaDataInformation)
        {
            var headerInformation = GetHeaderInformation(metaDataInformation[0]).ToArray();
            var dataInformation = metaDataInformation[1].Split(FormValidator.ValidValuesEntrySeparator, StringSplitOptions.RemoveEmptyEntries);
            var columns = new Dictionary<string, List<string>>();

            if (!_formValidator.IsRowFormatValid(dataInformation))
            {
                throw new CsvRowFormatInvalidException($"Rows from imported file did not match expected format.");
            }

            for (var i = 0; i < headerInformation.Length; i++)
            {
                var currentName = headerInformation[i];
                var currentColValues = new List<string>();

                foreach (var currentDataRow in dataInformation)
                {
                    var splitData = currentDataRow.Split(FormValidator.ValidValueSeparator, StringSplitOptions.RemoveEmptyEntries);
                    var data = splitData[i].ToLower();
                    currentColValues.Add(data);
                }

                columns.Add(currentName, currentColValues);
            }

            return columns;
        }

        /// <summary>
        /// Splits the header values from the row values at the Data-tag.
        /// </summary>
        /// <param name="fileContent">The file content.</param>
        /// <returns>Returns the split file content at the Data tag.</returns>
        private IEnumerable<string> SplitMetaDataInformationFromRawFileContent(string fileContent)
        {
            var rawMetaData = fileContent.Split(FormValidator.ValidDataSeparator);
            return rawMetaData;
        }
    }
}
