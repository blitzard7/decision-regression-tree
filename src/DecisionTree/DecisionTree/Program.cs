using DecisionTree.Logic.IoC;
using DecisionTree.Logic.Services;
using DecisionTree.Logic.Trees;
using DecisionTree.Logic.Validator;
using DecisionTree.UI;
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
            var fileService = container.GetService<IFileService>();
            var csvService = container.GetService<ICsvService>();
            var formValidator = container.GetService<IFormValidator>();
            var application = new Application(fileService, csvService, formValidator);
            application.Start();
        }
    }
}
