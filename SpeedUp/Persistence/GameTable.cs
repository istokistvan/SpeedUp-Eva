namespace SpeedUp.Persistence
{
    public class GameTable
    {
        #region Constructors

        public GameTable() : this(5,10) { }

        public GameTable(int gameSizeX, int gameSizeY)
        {
            if (gameSizeX < 0)
            {
                throw new ArgumentOutOfRangeException("The game size cannot be less, than 0!");
            }

            if (gameSizeY < 0)
            {
                throw new ArgumentOutOfRangeException("The game size cannot be less, than 0!");
            }

            Size = new (gameSizeX,gameSizeY);
            GameFields = new int[gameSizeX][];
            for (int i=0; i<gameSizeX; i++)
            {
                GameFields[i] = new int[gameSizeY];
            }
            Gas = 50;
        }

        #endregion

        #region Properties

        public GameTableSize Size { get; set; }

        public int[][] GameFields { get; set; }

        public int BikePos { get;  set; }

        public int GameTime { get; set; }

        public int Gas { get; set; }



        //mezők értékének beállítása
        //0: üres; 1: motor pozíciója; 2: benzin
        public void SetValue(int x, int y, int value)
        {
            if (x < 0)
            {
                throw new ArgumentOutOfRangeException("The x parameter cannot be less, than 0!");
            } else if (y < 0)
            { 
                throw new ArgumentOutOfRangeException("The y parameter cannot be less, than 0!");
            } else if (value < 0 || value > 2)
            {
                throw new ArgumentOutOfRangeException("The value parameter cannot be less, than 0, or bigger, than 2!");
            }

            GameFields[x][y] = value;
        }

        //mezők értékének lekérése
        //0: üres; 1: motor pozíciója; 2: benzin
        public int GetValue(int x, int y) 
        {
            if (x < 0 || x > Size.X)
            {
                throw new ArgumentOutOfRangeException("The X coordinate is out of range!");
            } 
            
            if (y < 0 || y > Size.Y)
            {
                throw new ArgumentOutOfRangeException("The Y coordinate is out of range!");
            }

            return GameFields[x][y];
        }

        public void StepValue(int x, int y)
        {
            if (x < 0 || x >= Size.X)
            {
                throw new ArgumentOutOfRangeException("The X coordinate is out of range.");
            }

            if (y < 0 || y >= Size.Y)
            {
                throw new ArgumentOutOfRangeException("The Y coordinate is out of range.");
            }

            if (y + 1 < Size.Y && GameFields[x][y + 1] == 0)
            {
                GameFields[x][y + 1] = 2;
            }
            else if (y + 1 < Size.Y && GameFields[x][y + 1] == 1)
            {
                Gas += 4;
            }
            GameFields[x][y] = 0;
        }

        #endregion

    }
}
