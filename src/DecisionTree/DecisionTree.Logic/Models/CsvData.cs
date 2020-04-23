using System.Collections.Generic;

namespace DecisionTree.Logic.Models
{
    public class CsvData
    {
        public CsvData()
        {
            Columns = new List<CsvColumn>();
            Values = new List<CsvRow>();
        }

        public List<CsvColumn> Columns { get; set; }
        public List<CsvRow> Values { get; set; }
    }
}
