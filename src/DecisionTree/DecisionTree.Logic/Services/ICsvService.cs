using DecisionTree.Logic.Models;
using System.Collections.Generic;

namespace DecisionTree.Logic.Services
{
    public interface ICsvService
    {
        CsvData CreateCsvDataFromFile(string file);
        
        IEnumerable<string> GetHeaderInformation(string file);

        IEnumerable<string> GetDataInformation(string file);

        Dictionary<string, List<string>> CreateColumns(string[] metaDataInformation);
    }
}
