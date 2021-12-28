namespace MapViewer.Services
{
    using MapViewer.Services.Data.AdminService;
    using MapViewer.Services.Dtos;
    using MapViewer.Services.Processors;
    using MapViewer.Services.Providers;
    using MapViewer.Services.SeedDatabase;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServicesConfigurator
    {
        public static void ConfigureAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IConnectionStringProvider>(new ConnectionStringProvider(config.GetValue<string>("ConnectionString:MapViewer")));
            services.AddScoped<IDatabaseProvider<IWorldDatabase>, DatabaseProvider<IWorldDatabase>>();
            services.AddScoped<IDatabaseProvider<IOtherDatabase>, DatabaseProvider<IOtherDatabase>>();
            services.AddTransient<IMapViewerAdminService, MapViewerAdminService>();
            services.AddTransient<IProcessTask, MapBuilderTask>();
        }

        public static void ConfigureSeedServices(this IServiceCollection services)
        {
            services.AddTransient<ISeedData, SeedWorlds>();
        }
    }
}
