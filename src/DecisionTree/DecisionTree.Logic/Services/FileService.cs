using System;
using System.IO;

namespace DecisionTree.Logic.Services
{
    public class FileService : IFileService
    {
        public void Export(string data)
        {
            // Export decision tree data?
            throw new NotImplementedException();
        }

        public string Import(string file)
        {
            if (!CheckFileExtentionToCsv(file))
            {
                throw new InvalidFileExtensionException();
            }
            var data = File.ReadAllText(file);

            if (data.Length <= 0)
            {
                return string.Empty;
            }

            return data;
        }

        private bool CheckFileExtentionToCsv(string file)
        {
            var extension = Path.GetExtension(file);

            if (string.IsNullOrEmpty(extension) || !extension.Equals(".csv")) 
            {
                return false;
            }

            return true;
        }
    }
}
