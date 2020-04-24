using System.Collections.Generic;

namespace DecisionTree.Logic.Models
{
    public class CsvData
    {
        public CsvData()
        {
            Columns = new Dictionary<string, List<string>>();
            Rows = new List<string>();
        }

        public Dictionary<string, List<string>> Columns { get; set; }
        public List<string> Rows { get; set; }
    }
}
