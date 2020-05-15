using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Logic.Helper
{
    /// <summary>
    /// Represents the Calculation class.
    /// </summary>
    public static class Calculation
    {
        /// <summary>
        /// Calculates the entropy.
        /// </summary>
        /// <param name="distinctResultValues">The distinct result values.</param>
        /// <param name="resultSetValues">The result set values.</param>
        /// <returns></returns>
        public static double CalculateEntropy(List<string> distinctResultValues, List<string> resultSetValues)
        {
            const int logBase = 2;
            var totalAmount = resultSetValues.Count;
            var occurrences = distinctResultValues.CalculateOccurenceOfGivenEntries(resultSetValues);
            var entropy = occurrences.Sum(x => -((double)x.Value / totalAmount) * Math.Log(((double)x.Value / totalAmount), logBase));

            return entropy;
        }

        /// <summary>
        /// Calculates the Information Gain (IG).
        /// </summary>
        /// <param name="totalEntropy">The total entropy.</param>
        /// <param name="featureEntropy">The feature entropy.</param>
        /// <returns></returns>
        public static double CalculateInformationGain(double totalEntropy, double featureEntropy) => totalEntropy - featureEntropy;
    }
}
