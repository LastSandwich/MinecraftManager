namespace MapViewer.Services.Processors
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;

    using MapViewer.Services.Dtos;
    using MapViewer.Services.Providers;

    using NPoco;

    public class MapBuilderTask : IProcessTask
    {
        private readonly IDatabase _database;

        public MapBuilderTask(IDatabaseProvider<IWorldDatabase> databaseProvider)
        {
            _database = databaseProvider.GetDatabase();
        }

        public int Order => 1;

        public string Name => "Map Builder";

        public TaskType TaskType => TaskType.MapBuilder;

        public async Task<ProcessResult> Run()
        {
            // Get list of worlds
            var worlds = await _database.QueryAsync<World>().ToList().ConfigureAwait(false);
            Console.WriteLine($"Found {worlds.Count} worlds to process");

            foreach (var world in worlds)
            {
                Console.WriteLine(world.Name);

                // get latest backup
                if (string.IsNullOrEmpty(world.BackupPath) || string.IsNullOrEmpty(world.OutputPath))
                {
                    continue;
                }

                var directoryInfo = new DirectoryInfo(world.BackupPath);
                var mostRecent = directoryInfo.GetFiles().OrderByDescending(p => p.Name).FirstOrDefault();
                //foreach (var file in directoryInfo.GetFiles().OrderByDescending(p => p.CreationTime))
                //{
                //    Console.WriteLine($"Found {file.Name} updated on {file.CreationTime.ToShortDateString()}");
                //}

                if (mostRecent == null)
                {
                    continue;
                }

                if (world.RenderedDate.HasValue && world.RenderedDate > mostRecent.CreationTimeUtc)
                {
                    Console.WriteLine("No updates since last render");
                    continue;
                }

                var tempFolderName = Guid.NewGuid().ToString();
                var tempPath = Path.GetTempPath();
                var tempFolder = Directory.CreateDirectory(Path.Combine(tempPath, "Papyrus", tempFolderName));

                // unzip
                ZipFile.ExtractToDirectory(mostRecent.FullName, tempFolder.FullName);

                // Run Papyrus
                await Render(tempFolder.FullName, world.OutputPath, 0).ConfigureAwait(false);
                await Render(tempFolder.FullName, world.OutputPath, 1).ConfigureAwait(false);
                await Render(tempFolder.FullName, world.OutputPath, 2).ConfigureAwait(false);
                Directory.Delete(tempFolder.FullName, true);

                // Update DB
                world.RenderedDate = DateTime.UtcNow;
                await _database.UpdateAsync(world).ConfigureAwait(false);

                // Publish
            }

            return ProcessResult.Error("No processing occurred");
        }

        private static async Task Render(string world, string output, int dimension)
        {
            Console.WriteLine($"Process {world}, dimension {dimension}");

            var renderer = new Process();
            renderer.StartInfo.FileName = @"C:\src\Papyrus-LastSandwich\PapyrusCs\bin\Debug\netcoreapp3.1\PapyrusCs.exe";
            renderer.StartInfo.WorkingDirectory = @"C:\src\Papyrus-LastSandwich\PapyrusCs\bin\Debug\netcoreapp3.1";
            renderer.StartInfo.Arguments = $"-w \"{world}\\db\" -o \"{output}\" -d {dimension}";
            renderer.StartInfo.RedirectStandardInput = true;
            renderer.Start();

            await renderer.WaitForExitAsync().ConfigureAwait(false);
        }
    }
}
