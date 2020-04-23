using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionTree.Logic.Models
{
    public class CsvColumn
    {
        public CsvColumn()
        {
            Values = new List<string>();
        }
        public string Name { get; set; }
        public List<string> Values { get; set; }
    }
}
