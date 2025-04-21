using System.Collections.Generic;

namespace DecisionTree.Logic.Interfaces
{
    public interface IFileService
    {
        /// <summary>
        /// Imports the given file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Returns the read file or an empty string if the imported data has no content.</returns>
        string Import(string file);

        /// <summary>
        /// Exports the entered column and row values into the given path.
        /// </summary>
        /// <param name="columns">The columns.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="path">The export file path.</param>
        string Export(string columns, IEnumerable<string> rows, string path);
    }
}
