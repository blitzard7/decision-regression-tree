using System.Collections.Generic;

namespace DecisionTree.Logic.Models
{
    public class CsvRow
    {
        public CsvRow()
        {
            Values = new List<string>();
        }

        public List<string> Values { get; set; }
    }
}
