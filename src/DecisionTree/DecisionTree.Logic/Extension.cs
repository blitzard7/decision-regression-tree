using System.Collections.Generic;
using System.Linq;
using DecisionTree.Logic.Models;

namespace DecisionTree.Logic
{
    /// <summary>
    /// Represents the Extensions class.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Gets the unique column values of the specified key.
        /// </summary>
        /// <param name="csvData">The csv data.</param>
        /// <param name="specificKey">The specified key.</param>
        /// <returns>Returns distinct column values according to the specified key.</returns>
        public static IEnumerable<string> GetUniqueColumnValues(this CsvData csvData, string specificKey)
        {
            var values = csvData.Columns[specificKey];
            return values.Distinct();
        }

        /// <summary>
        /// Gets specific row entries according to the given search string.
        /// </summary>
        /// <param name="csvData">The csv data.</param>
        /// <param name="searchString">The search string.</param>
        /// <returns>Returns filtered rows according to the given search string.</returns>
        public static IEnumerable<string> GetSpecificRowEntries(this CsvData csvData, string searchString) => csvData.Rows.Where(x => x.Contains(searchString));

        /// <summary>
        /// Calculates the occurence of the given distinct values in the given entries.
        /// </summary>
        /// <param name="distinctValues">The distinct values.</param>
        /// <param name="allElements">All elements</param>
        /// <returns>Returns a dictionary where the key is the distinct value and the value is the occurence of the key.</returns>
        public static Dictionary<string, int> CalculateOccurenceOfGivenEntries(this IEnumerable<string> distinctValues, IEnumerable<string> allElements)
        {
            var tmpDistinct = distinctValues.ToList();
            var tmpAllElements = allElements.ToList();
            var occurrences = new Dictionary<string, int>();

            for (var i = 0; i < tmpDistinct.Count; i++)
            {
                var current = tmpDistinct[i];
                var amount = tmpAllElements.Count(x => x.Contains(current));
                if (amount != 0)
                {
                    occurrences.Add(current, amount);
                }
            }

            return occurrences;
        }
    }
}
