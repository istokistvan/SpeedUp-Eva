using System.Text.Json;

namespace SpeedUp.Persistence
{
    public class DataAccess : IDataAccess
    {

        public async Task Save(Stream stream, GameTable dto) => await Task.Run(() => JsonSerializer.SerializeAsync(stream, dto));

        public async Task<GameTable?> Load(Stream stream) => await Task.Run(() => JsonSerializer.DeserializeAsync<GameTable>(stream).AsTask());
    }
}
