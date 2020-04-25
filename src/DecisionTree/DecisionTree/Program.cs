using DecisionTree.Logic.IoC;
using DecisionTree.Logic.Services;
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
            fileService.Import(file);
        }
    }
}
