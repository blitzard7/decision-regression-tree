using DecisionTree.Logic.Interfaces;
using DecisionTree.Logic.Services;
using DecisionTree.Logic.Validator;
using Microsoft.Extensions.DependencyInjection;

namespace DecisionTree.Logic.IoC
{
    public class IoCHelper
    {
        public static ServiceProvider RegisterDependencies(IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton<IFormValidator, FormValidator>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<ICsvService, CsvService>();
            return services.BuildServiceProvider();
        }
    }
}
