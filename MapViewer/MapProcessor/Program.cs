// See https://aka.ms/new-console-template for more information

using System.Reflection;

using MapViewer.Services;
using MapViewer.Services.Processors;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

return await Execute().ConfigureAwait(false);

static async Task<int> Execute()
{
    Console.ResetColor();
    var configuration = LoadConfiguration();
    var container = ConfigureServices(configuration);

    //var connectionStringProvider = container.GetService<IConnectionStringProvider>();
    var processors = container.GetServices<IProcessTask>();

    var allOk = true;
    foreach (var processTask in processors.OrderBy(x => x.Order))
    {
        if (!allOk)
        {
            continue;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Processing {processTask.Name}");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        var result = await processTask.Run().ConfigureAwait(false);
        allOk = allOk && result.IsSuccess;
        Console.ForegroundColor = result.IsSuccess ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"{(result.IsSuccess ? "Success!" : "Failed!")} {result.Message}");
        Console.ResetColor();
        Console.WriteLine(string.Empty);
    }

    return allOk ? 0 : -1;
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
    return services.BuildServiceProvider();
}
