using DecisionTree.Helper;
using DecisionTree.Logic.Models;
using DecisionTree.Logic.Services;
using DecisionTree.Logic.Trees;
using DecisionTree.Logic.Validator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DecisionTree.UI
{
    public class Application : IApplication
    {
        private readonly ICsvService _csvService;
        private readonly IFileService _fileService;
        private readonly IFormValidator _formValidator;
        private bool _isRunning;
        private string _csvFilePath;
        private CsvData _csvData;
        private ITree _tree;

        public Application(IFileService fileService, ICsvService csvService, IFormValidator formValidator)
        {
            _fileService = fileService;
            _csvService = csvService;
            _formValidator = formValidator;
        }

        public void Exit()
        {
            _isRunning = false;
        }

        public void Start()
        {
            _isRunning = true;

            DrawInterface();
            while (_isRunning)
            {
                // Display UserInterface and handle interaction
                var userInput = Console.ReadKey(true);
                HandleUserInput(userInput);
            }
        }

        private void DrawInterface()
        {
            ConsoleHelper.WriteLine(UserInterface.ImportData, ConsoleColor.Yellow);
            ConsoleHelper.WriteLine(UserInterface.ExportData, ConsoleColor.Yellow);
            ConsoleHelper.WriteLine(UserInterface.QueryTree, ConsoleColor.Yellow);
            ConsoleHelper.WriteLine(UserInterface.Help, ConsoleColor.Yellow);
            ConsoleHelper.WriteLine(UserInterface.DisplayMenu, ConsoleColor.Yellow);
            ConsoleHelper.WriteLine(UserInterface.PrintTree, ConsoleColor.Yellow);
            ConsoleHelper.WriteLine(UserInterface.Quit, ConsoleColor.Yellow);
        }

        private void HandleUserInput(ConsoleKeyInfo userInput)
        {
            switch(userInput.Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    _csvFilePath = ImportDataCommand();
                    _tree = StartCalculatingTree();
                    break;
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    // export data
                    ExportData();
                    break;
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    // extact columnames of csvfile since those should represent the search categories (exluding ResultCategory which is the last element)
                    QueryTree(_tree);
                    break;
                case ConsoleKey.NumPad4:
                case ConsoleKey.D4:
                    // display help
                    DisplayHelp();
                    break;
                case ConsoleKey.NumPad5:
                case ConsoleKey.D5:
                    // display menu
                    DrawInterface();
                    break;
                case ConsoleKey.NumPad6:
                case ConsoleKey.D6:
                    VisualiseTree(_tree);
                    break;
                case ConsoleKey.NumPad7:
                case ConsoleKey.D7:
                    _isRunning = false;
                    break;
                default:
                    // write that command in unknown.
                    break;
            }
        }

        private string ImportDataCommand()
        {
            ConsoleHelper.Write("Please enter path to csv file:");
            var input = Console.ReadLine();
            return input;
        }

        private void ExportData()
        {
            // user can add columns and rows including resultset 
            // this should be then exported as csvfile (accordingly to provided format) and user can (if he/she wants to) import it later.
            var columns = GetColumnInformationForExport();
            var rows = GetRowInformationForExport();

            // Todo check input
            var isGivenDataValid = CheckExportDataForValidity(columns, rows);

            if (!isGivenDataValid)
            {
                ConsoleHelper.WriteLine("Your entered data does not match the expected format.\r\nPlease make sure your rows value matches the columns and your columns and rows are \";\" separated.\r\nDo you want to quit? (Y/N)");
                var quit = Console.ReadKey(true);

                if (quit.Key == ConsoleKey.Y)
                {
                    return;
                }
                else if (quit.Key == ConsoleKey.N)
                {
                    ExportData();
                }
            }

            ConsoleHelper.Write("Enter path where to export your data: ");
            var path = Console.ReadLine();

            try
            {
                _fileService.Export(columns, rows, path);
            }
            catch (Exception)
            {
                ConsoleHelper.WriteLine($"An error has been encountered while exporting to {path}.");
            }
        }

        private string GetColumnInformationForExport()
        {
            ConsoleHelper.Write("Please enter your column name semi-colon \";\" separated: ");
            var input = Console.ReadLine();

            return input;
        }

        private List<string> GetRowInformationForExport()
        {
            var rows = new List<string>();
            ConsoleHelper.WriteLine("Please enter your row values semi-colon \";\" separated (press \"Enter\" to quit): ");

            string input;
            while (!string.IsNullOrEmpty(input = Console.ReadLine()))
            {
                rows.Add(input);
            }

            return rows;
        }

        private bool CheckExportDataForValidity(string columns, List<string> rows)
        {
            var columnsSeparatedCorrectly = _formValidator.IsDataSeparatedCorrectly(columns);
            var rowsSeparatedCorretly = rows.All(x => _formValidator.IsDataSeparatedCorrectly(x));
            var rowsFormatValid = _formValidator.IsRowFormatValid(rows.ToArray());

            return columnsSeparatedCorrectly && rowsSeparatedCorretly && rowsFormatValid;
        }

        private ITree StartCalculatingTree()
        {
            var fileName = Path.GetFileName(_csvFilePath);
            ConsoleHelper.WriteLine($"Starting tree building for file {fileName}");
            try
            {
                var data = _fileService.Import(_csvFilePath);
                _csvData = _csvService.CreateCsvDataFromFile(data);

            }
            catch (InvalidFileExtensionException)
            {
                ConsoleHelper.WriteLine($"File {fileName} has an invalid file extension. Make sure you upload a \".csv\" file.", ConsoleColor.Red);
                return null;
            }

            var dt = new Logic.Trees.DecisionTree();
            var tree = dt.BuildTree(_csvData);
            ConsoleHelper.WriteLine($"Finished tree building for file {fileName}");
            VisualiseTree(tree);
            return tree;
        }

        private void VisualiseTree(ITree tree)
        {
            if (tree == null || tree.Root == null)
            {
                ConsoleHelper.WriteLine("No tree present. Please make sure you upload data firt before printing the tree.", ConsoleColor.Red);
                return;
            }

            PrintTree(tree.Root, "", true);
        }

        private static void PrintTree(INode tree, string indent, bool last)
        {
            // if it is a Feature Category (ColumnName) then represent it as UpperCase.
            // features could be colorized 
            // leaves could be colorized
            string feature = null;
            if (tree.Feature != null)
            {
                feature = tree.Feature.ToUpper();
            }
            var name = tree.FeatureValue != null ? $"{tree.FeatureValue} -> {feature}" : feature;

            if (tree.IsLeaf)
            {
                name = $"{tree.FeatureValue} -> {tree.Result}";
            }

            Console.WriteLine(indent + "+- " + name);

            // if children, if node
            var a = tree.Parent != null && !tree.IsLeaf;
            indent += a ? "|  " : "   ";

            for (int i = 0; i < tree.Children.Count; i++)
            {
                var featureNode = tree.Children.ElementAt(i).Value;
                PrintTree(featureNode, indent, featureNode.IsLeaf);
            }
        }

        private INode QueryTree(ITree tree)
        {
            if (string.IsNullOrEmpty(_csvFilePath) || tree == null)
            {
                ConsoleHelper.WriteLine("Please import first a csv file before querying the tree.", ConsoleColor.Red);
                return null;
            }

            var headers = _csvData.Headers;
            INode foundNode = null;
            
            // header.count - 1 since we do not want to query resultcatergory.
            var searchKeys = new List<(string featureName, string featureValue)>();
            for (int i = 0; i < headers.Count - 1; i++)
            {
                var current = headers[i];

                // Todo display possible values for current column?
                Console.WriteLine($"Enter search value for feature {current.ToUpper()}: ");
                var searchInput = Console.ReadLine();

                // todo validate input
                searchKeys.Add((current, searchInput));
            }

            return foundNode;
        }

        private void DisplayHelp()
        {
            ConsoleHelper.WriteLine("Csv Schema", ConsoleColor.Cyan);
            ConsoleHelper.WriteLine("==========", ConsoleColor.Cyan);
            ConsoleHelper.WriteLine("Columns and Rows are separated by a \"Data\" tag.");
            ConsoleHelper.WriteLine("Without that \"Data\" tag the import will fail.\r\n");
            ConsoleHelper.Write("Columns: are separated by \";\"");
            ConsoleHelper.Write("Rows: are separated by \";\"");
            ConsoleHelper.WriteLine("Example:");
            ConsoleHelper.WriteLine("Col1;Col2;Col3;\r\nData\r\nrow1;row2;row3;\r\n");
            ConsoleHelper.WriteLine("Export Data", ConsoleColor.Cyan);
            ConsoleHelper.WriteLine("===========", ConsoleColor.Cyan);
            ConsoleHelper.WriteLine("You can enter your column and row values into the console.");
            ConsoleHelper.WriteLine("Make sure your column and row values are separated with \";\".");
            ConsoleHelper.WriteLine("When exporting data, there is no need to separate columns and rows with the \"Data\" tag.");
            ConsoleHelper.WriteLine("The \"Data\" tag will automatically be inserted when exporting to csv.\r\n");
            ConsoleHelper.WriteLine("Tree Visualization", ConsoleColor.Cyan);
            ConsoleHelper.WriteLine("==================", ConsoleColor.Cyan);
            ConsoleHelper.WriteLine("Tree Nodes: are represented in uppercase.");
            ConsoleHelper.WriteLine("Tree Edges: are represented in lowercase.");
            ConsoleHelper.WriteLine("Leaves: are represented in lowercase.");
            ConsoleHelper.WriteLine("The parent node wraps all child elements inside the \"|\"");
            ConsoleHelper.WriteLine("Node children are represented indented of the parent node.");
            ConsoleHelper.WriteLine("Example:");
            ConsoleHelper.WriteLine(@"+- SWIMMING SUIT
   +- None -> No
   +- Small -> No
   +- Good -> WATER TEMPERATURE
   |  +- Cold -> No
   |  +- Warm -> Yes");
        }
    }
}
