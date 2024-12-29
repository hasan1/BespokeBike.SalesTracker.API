using BespokeBike.SalesTracker.API.Service;
using BespokeBike.SalesTracker.API.Repository;
using BespokeBike.SalesTracker.API.Model;

namespace BespokeBike.SalesTracker.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind AppSettings
            var appSettingsSection = configuration.GetSection("ConnectionStrings");
            services.AddScoped<IAppSettings>(provider => new AppSettings(appSettingsSection["SalesTrackerDBConn"]!));

            // Register services
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ISaleService, SaleService>();

            // Register repositories           
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
        }
    }
}
