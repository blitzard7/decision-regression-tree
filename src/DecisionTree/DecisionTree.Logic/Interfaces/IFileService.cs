using System.Collections.Generic;

namespace DecisionTree.Logic.Interfaces
{
    public interface IFileService
    {
        string Import(string file);
        void Export(string columns, IEnumerable<string> rows, string path);
    }
}
