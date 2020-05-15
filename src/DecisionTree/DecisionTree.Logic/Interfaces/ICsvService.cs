using System.Collections.Generic;
using DecisionTree.Logic.Models;

namespace DecisionTree.Logic.Interfaces
{
    /// <summary>
    /// Represents the ICsvService.
    /// </summary>
    public interface ICsvService
    {
        /// <summary>
        /// Creates the csv data structure from the imported file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Returns the constructed <see cref="CsvData"/> from the imported file.</returns>
        CsvData CreateCsvDataFromFile(string file);

        /// <summary>
        /// Get the header information of the imported file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Returns the name of the columns.</returns>
        IEnumerable<string> GetHeaderInformation(string file);

        /// <summary>
        /// Get the row information of the imported file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Returns the rows.</returns>
        IEnumerable<string> GetRowValues(string file);

        /// <summary>
        /// Creates the columns containing the column name as key and its row values.
        /// </summary>
        /// <param name="metaDataInformation">The meta information of the file.</param>
        /// <returns>Returns columns.</returns>
        Dictionary<string, List<string>> CreateColumns(string[] metaDataInformation);
    }
}
