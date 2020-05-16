using DecisionTree.Logic.Interfaces;
using DecisionTree.Logic.Services;
using DecisionTree.Logic.Validator;
using Microsoft.Extensions.DependencyInjection;

namespace DecisionTree.Logic.IoC
{
    /// <summary>
    /// Represents the IoCHelper class.
    /// </summary>
    public class IoCHelper
    {
        /// <summary>
        /// Registers dependencies for dependency injection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>Returns the constructed service provider.</returns>
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
