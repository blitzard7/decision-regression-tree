using DecisionTree.Logic.Models;
using System.Collections.Generic;

namespace DecisionTree.Logic.Services
{
    public interface ICsvReader
    {
        string[] Read(string file);
    }
}
