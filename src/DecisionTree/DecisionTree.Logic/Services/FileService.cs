using System.Collections.Generic;
using System.IO;
using DecisionTree.Logic.Exceptions;
using DecisionTree.Logic.Interfaces;
using DecisionTree.Logic.Validator;

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
        /// <returns>Returns tha path where the file has been exported.</returns>
        public string Export(string columns, IEnumerable<string> rows, string path)
        {
            string exportPath = string.Empty;
            var lines = new List<string>
            {
                columns,
                FormValidator.ValidDataSeparator
            };

            lines.AddRange(rows);

            if (CheckIfPathIsDirectory(path) )
            {
                exportPath = HandleExportForDirectoryPathInput(path, lines);
            }
            else
            {
                exportPath = HandleExportForFilePathInput(path, lines);
            }

            return exportPath;
        }

        private bool CheckIfPathIsDirectory(string path)
        {
            var fileAttribute = File.GetAttributes(path);
            return fileAttribute.HasFlag(FileAttributes.Directory);
        }

        /// <summary>
        /// Handles the export of the entered data when path is pointing to a directory.
        /// A new file with the name "exportData.csv" will be created.
        /// </summary>
        /// <param name="path">The directory path.</param>
        /// <param name="lines">The lines to export.</param>
        /// <returns>Returns the path where the file has been exported.</returns>
        private string HandleExportForDirectoryPathInput(string path, IEnumerable<string> lines)
        {
            const string fileName = "exportData.csv";
            var filePath = Path.Combine(path, fileName);
            File.WriteAllLines(filePath, lines);
            return filePath;
        }

        /// <summary>
        /// Handles the export of the entered data when path is pointing to a file.
        /// If the data already exists, the content will be overwritten with the new one.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="lines">The lines to export.</param>
        /// <returns>Returns the path where the file has been exported.</returns>
        private string HandleExportForFilePathInput(string path, IEnumerable<string> lines)
        {
            File.WriteAllLines(path, lines);
            return path;
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
