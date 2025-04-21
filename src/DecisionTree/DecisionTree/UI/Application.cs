using DecisionTree.Helper;
using DecisionTree.Logic.Exceptions;
using DecisionTree.Logic.Interfaces;
using DecisionTree.Logic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DecisionTree.UI
{
    /// <summary>
    /// Represents the Application class.
    /// This class is used to construct the UI and user interaction.
    /// </summary>
    public class Application : IApplication
    {
        /// <summary>
        /// The csv service.
        /// </summary>
        private readonly ICsvService _csvService;

        /// <summary>
        /// The file service.
        /// </summary>
        private readonly IFileService _fileService;

        /// <summary>
        /// The form validator.
        /// </summary>
        private readonly IFormValidator _formValidator;

        /// <summary>
        /// The file name.
        /// </summary>
        private string _fileName;

        /// <summary>
        /// A value indicating whether the application is running or not.
        /// </summary>
        private bool _isRunning;

        /// <summary>
        /// The csv data.
        /// </summary>
        private CsvData _csvData;

        /// <summary>
        /// The tree.
        /// </summary>
        private ITree _tree;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="fileService">The file service.</param>
        /// <param name="csvService">The csv service.</param>
        /// <param name="formValidator">The form validator.</param>
        public Application(IFileService fileService, ICsvService csvService, IFormValidator formValidator)
        {
            _fileService = fileService;
            _csvService = csvService;
            _formValidator = formValidator;
        }

        /// <summary>
        /// Exit will quit the application.
        /// </summary>
        public void Exit()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Starts drawing the UI and handles user interaction.
        /// </summary>
        public void Start()
        {
            _isRunning = true;

            DrawInterface();
            while (_isRunning)
            {
                var userInput = Console.ReadKey(true);
                HandleUserInput(userInput);
            }
        }

        /// <summary>
        /// Draws the provided commands into the console.
        /// </summary>
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

        /// <summary>
        /// Handles the current user input.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        private void HandleUserInput(ConsoleKeyInfo userInput)
        {
            switch (userInput.Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                {
                    if (ImportDataCommand())
                    {
                        StartCalculatingTree();
                    }

                    break;
                }
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                {
                    ExportData();
                    break;
                }
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                {
                    QueryTree(_tree);
                    break;
                }
                case ConsoleKey.NumPad4:
                case ConsoleKey.D4:
                {
                    DisplayHelp();
                    break;
                }
                case ConsoleKey.NumPad5:
                case ConsoleKey.D5:
                {
                    DrawInterface();
                    break;
                }
                case ConsoleKey.NumPad6:
                case ConsoleKey.D6:
                {
                    VisualizeTree(_tree);
                    break;
                }
                case ConsoleKey.NumPad7:
                case ConsoleKey.D7:
                {
                    _isRunning = false;
                    break;
                }
            }
        }

        /// <summary>
        /// Prompts the user to import data for constructing the decision tree.
        /// </summary>
        /// <returns>Returns a value indicating whether the import of data was successfully or not.</returns>
        private bool ImportDataCommand()
        {
            ConsoleHelper.Write("Please enter path to csv file:");
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                ConsoleHelper.WriteLine("No path has been entered. Make sure you enter a path.", ConsoleColor.Red);
                return false;
            }

            _fileName = Path.GetFileName(input);

            try
            {
                var data = _fileService.Import(input);
                _csvData = _csvService.CreateCsvDataFromFile(data);
            }
            catch (FeatureNotFoundException)
            {
                ConsoleHelper.WriteLine(
                    $"File {_fileName} has an invalid file extension. Make sure you upload a \".csv\" file.", ConsoleColor.Red);
                return false;
            }
            catch (CsvDataInvalidMetadataException ex)
            {
                ConsoleHelper.WriteLine($"Importing file {_fileName} encountered an error: {ex.Message}", ConsoleColor.Red);
                return false;
            }
            catch (CsvRowFormatInvalidException)
            {
                ConsoleHelper.WriteLine($"Rows of {_fileName} does not match with expected format.", ConsoleColor.Red);
                return false;
            }
            catch (Exception)
            {
                ConsoleHelper.WriteLine($"While parsing {_fileName} an unexpected error has been encountered.", ConsoleColor.Red);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Starts calculating the tree with the provided import data and visualizes the results.
        /// </summary>
        private void StartCalculatingTree()
        {
            if (_csvData == null)
            {
                ConsoleHelper.WriteLine($"Parsed data for imported file  {_fileName} is empty.", ConsoleColor.Red);
            }

            ConsoleHelper.WriteLine($"Starting building tree...", ConsoleColor.DarkGray);
            var dt = new Logic.Trees.DecisionTree();

            try
            {
                _tree = dt.BuildTree(_csvData);
            }
            catch (Exception)
            {
                ConsoleHelper.WriteLine("While building the decision tree an error has been encountered.", ConsoleColor.Red);
                return;
            }

            VisualizeTree(_tree);

            ConsoleHelper.WriteLine($"Finished building tree...", ConsoleColor.DarkGray);
        }

        /// <summary>
        /// Prompts the user to enter column and row values for exporting data.
        /// </summary>
        private void ExportData()
        {
            var columns = GetColumnInformationForExport();
            var rows = GetRowInformationForExport();
            var isGivenDataValid = CheckExportDataForValidity(columns, rows);

            if (!isGivenDataValid)
            {
                ConsoleHelper.WriteLine("Your entered data does not match the expected format.\r\nPlease make sure your rows value matches the columns and your columns and rows are \";\" separated.\r\nDo you want to quit? (Y/N)");
                var quit = Console.ReadKey(true);

                if (quit.Key == ConsoleKey.Y)
                {
                    return; 
                }

                if (quit.Key == ConsoleKey.N)
                {
                    ExportData();
                }
            }

            ConsoleHelper.Write("Enter path where to export your data: ");
            var path = Console.ReadLine();
            var exportPath = string.Empty;
            try
            {
                exportPath = _fileService.Export(columns, rows, path);
            }
            catch (Exception)
            {
                ConsoleHelper.WriteLine($"An error has been encountered while exporting to {path}.");
            }

            ConsoleHelper.WriteLine($"File has been successfully exported to {exportPath}", ConsoleColor.DarkYellow);
        }

        /// <summary>
        /// Gets column information from the user for exporting data.
        /// </summary>
        /// <returns>Returns the retrieved column information.</returns>
        private string GetColumnInformationForExport()
        {
            ConsoleHelper.Write("Please enter header names semi-colon \";\" separated: ");
            var input = Console.ReadLine();

            return input;
        }

        /// <summary>
        /// Gets row information from the user for exporting data.
        /// </summary>
        /// <returns>Returns a <see cref="List{T}"/> of strings containing the row values.</returns>
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

        /// <summary>
        /// Checks whether or not the export data is valid or not.
        /// </summary>
        /// <param name="columns">The column value.</param>
        /// <param name="rows">The rows.</param>
        /// <returns>Returns a value indicating whether the data for export is valid or not.</returns>
        private bool CheckExportDataForValidity(string columns, List<string> rows)
        {
            var columnsSeparatedCorrectly = _formValidator.IsRowDataDelimiterValid(columns);
            var rowsSeparatedCorrectly = rows.All(x => _formValidator.IsRowDataDelimiterValid(x));
            var rowsFormatValid = _formValidator.IsRowFormatValid(rows.ToArray());

            return columnsSeparatedCorrectly && rowsSeparatedCorrectly && rowsFormatValid;
        }

        /// <summary>
        /// Queries the constructed tree.
        /// </summary>
        /// <param name="tree">The constructed tree.</param>
        /// <returns>Returns a node containing the result of the query.</returns>
        private void QueryTree(ITree tree)
        {
            if (tree == null)
            {
                ConsoleHelper.WriteLine("Please import first a csv file before querying the tree.", ConsoleColor.Red);
                return;
            }

            var headers = _csvData.Headers;
            INode foundNode;

            var searchKeys = new Dictionary<string, string>();
            for (var i = 0; i < headers.Count - 1; i++)
            {
                var current = headers[i];

                Console.WriteLine($"Enter search value for feature {current.ToUpper()}: ");
                var searchInput = Console.ReadLine();

                searchKeys.Add(current, searchInput);
            }

            try
            {
                foundNode = tree.Query(searchKeys);
            }
            catch (InvalidOperationException ex)
            {
                ConsoleHelper.WriteLine($"{ex.Message}", ConsoleColor.Red);
                return;
            }
            catch (FeatureNotFoundException ex)
            {
                ConsoleHelper.WriteLine($"{ex.Message}", ConsoleColor.Red);
                return;
            }

            DisplayQueryResult(foundNode);
        }

        /// <summary>
        /// Displays the query result into the console.
        /// </summary>
        /// <param name="result">The result node.</param>
        private void DisplayQueryResult(INode result)
        {
            ConsoleHelper.WriteLine("Querying the tree with the provided search categories resulted into following category:", ConsoleColor.DarkYellow);
            ConsoleHelper.WriteLine(result?.Result, ConsoleColor.Green);
        }

        /// <summary>
        /// Visualizes the tree in the console.
        /// </summary>
        /// <param name="tree">The constructed tree.</param>
        private void VisualizeTree(ITree tree)
        {
            if (tree?.Root == null)
            {
                ConsoleHelper.WriteLine("No tree present. Please make sure you upload data firt before printing the tree.", ConsoleColor.Red);
                return;
            }

            PrintTree(tree.Root, string.Empty);
        }

        /// <summary>
        /// Prints the tree recursively in the console.
        /// </summary>
        /// <param name="tree">The tree node.</param>
        /// <param name="indent">The indent of the current tree node.</param>
        private void PrintTree(INode tree, string indent)
        {
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

            var indentCondition = tree.Parent != null && !tree.IsLeaf;
            indent += indentCondition ? "|  " : "   ";

            for (var i = 0; i < tree.Children.Count; i++)
            {
                var featureNode = tree.Children.ElementAt(i).Value;
                PrintTree(featureNode, indent);
            }
        }

        /// <summary>
        /// Displays helpful information in the console.
        /// </summary>
        private void DisplayHelp()
        {
            ConsoleHelper.WriteLine("Csv Schema", ConsoleColor.Cyan);
            ConsoleHelper.WriteLine("==========", ConsoleColor.Cyan);
            ConsoleHelper.WriteLine("Headers and Rows are separated by a \"Data\" tag.");
            ConsoleHelper.WriteLine("Without that \"Data\" tag the import will fail.");
            ConsoleHelper.WriteLine("Headers: are separated by \";\"");
            ConsoleHelper.WriteLine("Rows: are separated by \";\"");
            ConsoleHelper.WriteLine("Example:");
            ConsoleHelper.WriteLine("Col1;Col2;Col3\r\nData\r\nrow1;row2;row3\r\n");
            ConsoleHelper.WriteLine("Export Data", ConsoleColor.Cyan);
            ConsoleHelper.WriteLine("===========", ConsoleColor.Cyan);
            ConsoleHelper.WriteLine("You can enter your header and row values into the console.");
            ConsoleHelper.WriteLine("Make sure your header and row values are separated with \";\".");
            ConsoleHelper.WriteLine("When exporting data, there is no need to separate header and rows with the \"Data\" tag.");
            ConsoleHelper.WriteLine("The \"Data\" tag will automatically be inserted when exporting to csv.\r\n");
            ConsoleHelper.WriteLine("Tree Visualization", ConsoleColor.Cyan);
            ConsoleHelper.WriteLine("==================", ConsoleColor.Cyan);
            ConsoleHelper.WriteLine("Tree Nodes: are represented in uppercase.");
            ConsoleHelper.WriteLine("Tree Nodes are demonstrated as [feature-value] -> [FEATURE] or [FEATURE] if it is the root node.");
            ConsoleHelper.WriteLine("Leaves: are demonstrated as [feature-value] -> [result]");
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
