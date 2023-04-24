namespace SpeedUp.Persistence
{
    public interface IDataAccess
    {
        public Task Save(Stream stream, GameTable dto);

        public Task<GameTable?> Load(Stream stream);
    }
}
