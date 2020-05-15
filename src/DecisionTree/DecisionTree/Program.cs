using DecisionTree.Logic.Interfaces;
using DecisionTree.Logic.IoC;
using DecisionTree.UI;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DecisionTree
{
    /// <summary>
    /// Represents the Program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Represents the entry point of the application.
        /// </summary>
        /// <param name="args">The command arguments.</param>
        public static void Main(string[] args)
        {
            Console.Title = "Adaptive Software Systems - Decision Tree (Project 1)";
            var services = new ServiceCollection();
            var container = IoCHelper.RegisterDependencies(services);
            var fileService = container.GetService<IFileService>();
            var csvService = container.GetService<ICsvService>();
            var formValidator = container.GetService<IFormValidator>();
            var application = new Application(fileService, csvService, formValidator);
            application.Start();
        }
    }
}
