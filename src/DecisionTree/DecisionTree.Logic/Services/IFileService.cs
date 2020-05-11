namespace DecisionTree.Logic.Services
{
    public interface IFileService
    {
        string Import(string file);
        void Export(string data);
    }
}
