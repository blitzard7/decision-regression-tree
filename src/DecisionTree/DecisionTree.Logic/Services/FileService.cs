using DecisionTree.Logic.Validator;
using System;
using System.Collections.Generic;
using System.IO;

namespace DecisionTree.Logic.Services
{
    public class FileService : IFileService
    {
        public void Export(string columns, IEnumerable<string> rows, string path)
        {
            var lines = new List<string>
            {
                columns,
                FormValidator.ValidDataSeparator
            };
            lines.AddRange(rows);
            File.AppendAllLines(path, lines);
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
