using DecisionTree.Logic.Exceptions;
using DecisionTree.Logic.Interfaces;
using DecisionTree.Logic.Validator;
using System.Collections.Generic;
using System.IO;

namespace DecisionTree.Logic.Services
{
    public class FileService : IFileService
    {
        /// <summary>
        /// Imports the given file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Returns the read file or an empty string if the imported data has no content.</returns>
        public string Import(string file)
        {
            if (!CheckFileExtensionToBeCsv(file))
            {
                throw new InvalidFileExtensionException();
            }
            var data = File.ReadAllText(file);

            return data.Length <= 0 ? string.Empty : data;
        }

        /// <summary>
        /// Exports the entered column and row values into the given path.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="path">The export file path.</param>
        public void Export(string columns, IEnumerable<string> rows, string path)
        {
            var lines = new List<string>
            {
                columns,
                FormValidator.ValidDataSeparator
            };
            lines.AddRange(rows);
            File.AppendAllLines(path, lines);
        }

        /// <summary>
        /// Checks whether or not the file is a .csv file or not.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Returns a value indicating whether or not the file has a .csv extension.</returns>
        private bool CheckFileExtensionToBeCsv(string file)
        {
            const string validFormat = ".csv";
            var extension = Path.GetExtension(file);

            return !string.IsNullOrEmpty(extension) && extension.Equals(validFormat);
        }
    }
}
