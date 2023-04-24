namespace SpeedUp.Persistence
{
    public class GameTableSize
    {
        public GameTableSize(int gameSizeX, int gameSizeY)
        {
            X = gameSizeX;
            Y = gameSizeY;
        }

        public GameTableSize() { }

        #region Properties

        public int X { get; set; }
        public int Y { get; set; }

        #endregion
    }
}
