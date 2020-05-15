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
        /// <returns>Returns the constructed <see cref="CsvData"/> from the imported file.</returns>
        public CsvData CreateCsvDataFromFile(string file)
        {
            if (!_formValidator.IsMetaInformationFormatValid(file))
            {
                throw new CsvDataInvalidMetadataException();
            }

            var metaDataInformation = SplitMetaDataInformationFromRawFileContent(file).ToArray();
            var headerInformation = GetHeaderInformation(metaDataInformation[0]).ToList();
            var columns = CreateColumns(metaDataInformation);
            var rows = GetRowValues(metaDataInformation[1]).ToList();

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
            const string separator = FormValidator.ValidValueSeparator;
            var header = SplitDataAtGivenCharacter(file, separator).ToList();

            if (header.Contains("\r\n"))
            {
                header.Remove(FormValidator.ValidValuesEntrySeparator);
            }

            var columns = header.Select(x => x.Trim('\r', '\n'));

            return !file.Contains(separator) ? Array.Empty<string>() : columns;
        }

        /// <summary>
        /// Get the row information of the imported file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Returns the rows.</returns>
        public IEnumerable<string> GetRowValues(string file)
        {
            const string separator = FormValidator.ValidValuesEntrySeparator;
            var data = SplitDataAtGivenCharacter(file, separator);
            return data;
        }

        public bool IsImportedFileValid(string file)
        {
            if (file.Length <= 0)
            {
                return false;
            }

            if (!file.Contains(FormValidator.ValidDataSeparator))
            {
                return false;
            }

            return !file.StartsWith(FormValidator.ValidDataSeparator);
        }

        public string[] SplitDataAtGivenCharacter(string data, string separator) => data.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// Creates the columns containing the column name as key and its row values.
        /// </summary>
        /// <param name="metaDataInformation">The metaDataInformation.</param>
        /// <returns>Returns columns.</returns>
        public Dictionary<string, List<string>> CreateColumns(string[] metaDataInformation)
        {
            var headerInformation = GetHeaderInformation(metaDataInformation[0]).ToArray();
            var dataInformation = SplitDataAtGivenCharacter(metaDataInformation[1], "\r\n").ToArray();
            var columns = new Dictionary<string, List<string>>();

            if (!_formValidator.IsRowFormatValid(dataInformation))
            {
                // throw invalid row exception?
                return columns;
            }

            for (int i = 0; i < headerInformation.Length; i++)
            {
                var currentName = headerInformation[i];
                var currentColValues = new List<string>();

                for (int j = 0; j < dataInformation.Length; j++)
                {
                    // validate column length and row data length 
                    var currentDataRow = dataInformation[j];
                    var splitData = SplitDataAtGivenCharacter(currentDataRow, FormValidator.ValidValueSeparator);
                    var data = splitData[i];
                    currentColValues.Add(data);
                }

                columns.Add(currentName, currentColValues);
            }

            return columns;
        }

        private IEnumerable<string> SplitMetaDataInformationFromRawFileContent(string fileContent)
        {
            const string separator = FormValidator.ValidDataSeparator;
            var rawMetaData = SplitDataAtGivenCharacter(fileContent, separator);
            return rawMetaData;
        }
    }
}
