using DecisionTree.Logic.Models;
using DecisionTree.Logic.Validator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Logic.Services
{
    public class CsvService : ICsvService
    {
        private readonly IFormValidator _formValidator;

        public CsvService(IFormValidator formValidator)
        {
            _formValidator = formValidator ?? throw new ArgumentNullException(nameof(formValidator));
        }

        public CsvData CreateCsvDataFromFile(string file)
        {
            if (!_formValidator.IsMetaInformationFormatValid(file))
            {
                throw new CsvInvalidException();
            }

            var metaDataInformation = SplitMetaDataInformationFromRawFileContent(file).ToArray();
            var columns = CreateColumns(metaDataInformation);        
            var rows = GetDataInformation(metaDataInformation[1]).ToList();

            var csvData = new CsvData
            {
                Columns = columns,
                Rows = rows
            };

            return csvData;
        }

        public IEnumerable<string> GetHeaderInformation(string headerMetaData)
        {
            var separator = FormValidator.ValidValueSeparator;
            var header = SplitDataAtGivenCharacter(headerMetaData, separator);
            if (!headerMetaData.Contains(separator))
            {
                return Array.Empty<string>();
            }

            return header;
        }

        public IEnumerable<string> GetDataInformation(string file)
        {
            var separator = FormValidator.ValidValueSeparator;
            if (!file.Contains(separator))
            {
                return Array.Empty<string>();
            }

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

            if (file.StartsWith(FormValidator.ValidDataSeparator))
            {
                return false;
            }

            return true;
        }

        public string[] SplitDataAtGivenCharacter(string data, string separator) => data.Split(separator, StringSplitOptions.RemoveEmptyEntries);

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
                    var data = SplitDataAtGivenCharacter(currentDataRow, FormValidator.ValidValueSeparator)[i];
                    currentColValues.Add(data);
                }

                columns.Add(currentName, currentColValues);
            }

            return columns;
        }

        private IEnumerable<string> SplitMetaDataInformationFromRawFileContent(string fileContent)
        {
            var separator = FormValidator.ValidDataSeparator;
            var rawMetaData = SplitDataAtGivenCharacter(fileContent, separator);
            return rawMetaData;
        }
    }
}
