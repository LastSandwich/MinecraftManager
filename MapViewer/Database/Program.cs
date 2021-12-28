// See https://aka.ms/new-console-template for more information

using System.Reflection;

using MapViewer.Services;
using MapViewer.Services.Helpers;
using MapViewer.Services.Providers;
using MapViewer.Services.SeedDatabase;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

return await Execute();

static async Task<int> Execute()
{
    var configuration = LoadConfiguration();
    var container = ConfigureServices(configuration);

    var connectionStringProvider = container.GetService<IConnectionStringProvider>();
    var seedCreators = container.GetServices<ISeedData>();

    DatabaseHelper.ResetDatabase(connectionStringProvider!.MapViewerConnectionString);
    var upgradeResult = DatabaseHelper.UpgradeDatabase(connectionStringProvider.MapViewerConnectionString, Assembly.GetExecutingAssembly());

    if (!upgradeResult.Successful)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(upgradeResult.Error);
        Console.ResetColor();
#if DEBUG
        Console.ReadLine();
#endif
        return -1;
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Success!");
    Console.ResetColor();

    await DatabaseHelper.SeedDatabase(seedCreators);

    return 0;
}

static IConfiguration LoadConfiguration()
{
    Console.WriteLine(Assembly.GetExecutingAssembly().Location);
    return new ConfigurationBuilder().AddUserSecrets("2b006bc9-cead-4837-a7ce-fffadbd013ea")
        .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
        .AddJsonFile("appsettings.json", false, true)
        .Build();
}

static ServiceProvider ConfigureServices(IConfiguration configuration)
{
    var services = new ServiceCollection();
    services.AddSingleton(configuration);
    services.ConfigureAppServices(configuration);
    services.ConfigureSeedServices();
    return services.BuildServiceProvider();
}
