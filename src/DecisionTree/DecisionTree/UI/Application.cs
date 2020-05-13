using DecisionTree.Helper;
using DecisionTree.Logic.Models;
using DecisionTree.Logic.Services;
using DecisionTree.Logic.Trees;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DecisionTree.UI
{
    public class Application : IApplication
    {
        private readonly IServiceProvider _container;
        private bool _isRunning;
        private string _csvFilePath;
        private CsvData _csvData;
        private ITree _tree;

        public Application(IServiceProvider container)
        {
            _container = container;
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
            ConsoleHelper.WriteLine(UserInterface.QueryTree, ConsoleColor.Yellow);
            ConsoleHelper.WriteLine(UserInterface.Help, ConsoleColor.Yellow);
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
                    break;
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    // extact columnames of csvfile since those should represent the search categories (exluding ResultCategory which is the last element)
                    QueryTree(_tree);
                    break;
                case ConsoleKey.NumPad4:
                case ConsoleKey.D4:
                    // display help
                    break;
                case ConsoleKey.NumPad5:
                case ConsoleKey.D5:
                    // display menu
                    break;
                case ConsoleKey.NumPad6:
                case ConsoleKey.D6:
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
        }

        private ITree StartCalculatingTree()
        {
            var fileName = Path.GetFileName(_csvFilePath);
            ConsoleHelper.WriteLine($"Starting tree building for file {fileName}");
            var fileService = _container.GetService<IFileService>();
            var data = fileService.Import(_csvFilePath);
            var csvService = _container.GetService<ICsvService>();
            _csvData = csvService.CreateCsvDataFromFile(data);
            var dt = new Logic.Trees.DecisionTree();
            var tree = dt.BuildTree(_csvData);
            ConsoleHelper.WriteLine($"Finished tree building for file {fileName}");
            VisualiseTree(tree.Root);
            return tree;
        }

        private void VisualiseTree(INode node)
        {
            PrintTree(node, "", true);
        }

        public static void PrintTree(INode tree, string indent, bool last)
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
            var headers = _csvData.Headers;
            INode foundNode = null;
            if (string.IsNullOrEmpty(_csvFilePath))
            {
                ConsoleHelper.WriteLine("Please import first a csv file before querying the tree.");
            }

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
    }
}
