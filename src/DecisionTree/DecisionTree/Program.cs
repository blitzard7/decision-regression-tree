using DecisionTree.Logic.IoC;
using DecisionTree.Logic.Services;
using DecisionTree.Logic.Trees;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DecisionTree
{
    public class Program
    {
        private static IServiceProvider container;

        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            container = IoCHelper.RegisterDependencies(services);
            Console.WriteLine("Enter path to csv file to construct a Decision Tree:");
            var file = Console.ReadLine();

            var fileService = container.GetService<IFileService>();
            var data = fileService.Import(file);

            var csvService = container.GetService<ICsvService>();
            var csvData = csvService.CreateCsvDataFromFile(data);
            var dt = new Logic.Trees.DecisionTree();
            var tree = dt.BuildTree(csvData);
            PrintTree(tree.Root);
            Console.WriteLine("Bla");
        }

        private static void PrintTree(INode node)
        {
            foreach (var item in node.Children)
            {
                Console.WriteLine($"{item.Value.Feature} --> {item.Key} --> ");
                foreach (var child in item.Value.Children)
                {
                    Console.WriteLine($"{child.Key}");
                }
            }
        }
    }
}
