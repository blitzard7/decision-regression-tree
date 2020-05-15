using DecisionTree.Logic.Models;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Logic
{
    public static class Extension
    {
        public static IEnumerable<string> GetUniqueColumnValues(this CsvData csvData, string specificKey)
        {
            var values = csvData.Columns[specificKey];
            return values.Distinct();
        }

        public static IEnumerable<string> GetSpecificRowEntries(this CsvData csvData, string searchString) => csvData.Rows.Where(x => x.Contains(searchString));

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
