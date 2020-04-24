using DecisionTree.Logic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DecisionTree.Logic.Services
{
    public class CsvReader : ICsvReader
    {
        public string[] Read(string file)
        {
            var data = File.ReadAllLines(file);

            if (data.Length <= 0)
            {
                return Array.Empty<string>();
            }

            return data;
        }
    }
}
