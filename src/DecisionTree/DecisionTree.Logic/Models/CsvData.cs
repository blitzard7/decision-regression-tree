using System.Collections.Generic;

namespace DecisionTree.Logic.Models
{
    public class CsvData
    {
        public List<CsvColumn> Columns { get; set; }
        public List<CsvRow> Values { get; set; }
    }
}
