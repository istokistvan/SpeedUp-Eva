using SpeedUp.Persistence;

namespace GameForm_MAUI.Persistence
{
    public class Store : IStore
    {
        public Task<IEnumerable<String>> GetFilesAsync()
        {
            return Task.Run(() => Directory.GetFiles(FileSystem.AppDataDirectory)
            .Select(Path.GetFileName)
            .Where(name => name?.EndsWith(".json") ?? false)
            .OfType<String>());
        }

        public async Task<DateTime> GetModifiedTimeAsync(string name)
        {
            var info = new FileInfo(Path.Combine(FileSystem.AppDataDirectory, name));

            return await Task.Run(() => info.LastWriteTime);
        }
    }
}
