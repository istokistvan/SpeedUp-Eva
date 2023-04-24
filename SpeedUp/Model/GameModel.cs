using SpeedUp.Persistence;

namespace SpeedUp.Model
{
    public class GameModel
    {
        #region Events

        public event EventHandler GameOver;

        public event EventHandler RefreshTable;

        public event EventHandler<int> GasValueChange;

        #endregion

        #region Fields

        private GameTable _gameTable;
        private int _timerCount = 0;
        private IDataAccess _dataAccess;
        private int _tickCounter = 0;

        #endregion

        #region Properties

        public GameTable GameTable { get => _gameTable; }

        public int TimerCount { get => _timerCount; set => _timerCount = (int)value; }

        public bool IsOver { get; private set; } = false;

        public double LifeTime { get => TimerCount * 1.0 / 1000; }

        #endregion

        #region Constructor

        public GameModel(IDataAccess dataAccess)
	    {
            _gameTable = new GameTable();
            _dataAccess = dataAccess;
	    }

        #endregion

        #region Methods

        private void GenerateFields()
        {
            for (int y = 0; y < _gameTable.Size.Y; y++)
            {
                for (int x = 0; x < _gameTable.Size.X; x++)
                {
                    _gameTable.SetValue(x, y, 0);
                    
                }
            }

            _gameTable.SetValue(_gameTable.Size.X / 2, _gameTable.Size.Y - 1, 1);
            _gameTable.BikePos = _gameTable.Size.X / 2;
        }

        private void GenerateGas()
        {
            Random rand = new Random();
            _gameTable.SetValue(rand.Next(_gameTable.Size.X), 0, 2);
        }

        public void NewGame()
        {
            _gameTable = new GameTable();
            GenerateFields();
            GenerateGas();
            IsOver = false;
        }

        public async Task SaveGame(Stream path)
        {
            if (_dataAccess == null)
            {
                throw new ArgumentNullException();
            }

            _gameTable.GameTime = TimerCount;

            await _dataAccess.Save(path,_gameTable);
        }

        public async Task LoadGame(Stream path)
        {
            if (_dataAccess == null)
            {
                throw new ArgumentNullException();
            }

            _gameTable = await Task.Run(() => _dataAccess.Load(path));
            _timerCount = _gameTable.GameTime;
            OnRefreshTable();
        }

        public void ProgressTime()
        {
            if (IsOver)
            {
                onGameOver();
            }
            
            Step();
            GasValueChange?.Invoke(this, _gameTable.Gas);

            if (_tickCounter % 2 == 0)
            {
                GenerateGas();
            }
            OnRefreshTable();
        }

        private void OnRefreshTable()
        {
            if(RefreshTable != null)
            {
                RefreshTable.Invoke(this, EventArgs.Empty);
            }
        }

        private void onGameOver()
        {
            if (GameOver != null)
            {
                GameOver.Invoke(this, EventArgs.Empty);
            }
        }

        private void Step()
        {
            for (int y = _gameTable.Size.Y - 1; y >= 0; y--)
            {
                for (int x = 0; x < _gameTable.Size.X; x++)
                {
                    if (_gameTable.GetValue(x, y) == 2)
                    {
                        _gameTable.StepValue(x, y);
                    }

                    if (_gameTable.Gas <= 0)
                    {
                        IsOver = true;
                    }
                }
            }
            _tickCounter++;
            _gameTable.Gas--;
        }

        #endregion

        #region Moving

        public void RightMove()
        {
            if (_gameTable.BikePos + 1 < _gameTable.Size.X)
            {
                _gameTable.SetValue(_gameTable.BikePos, _gameTable.Size.Y - 1, 0);
                _gameTable.BikePos++;
                _gameTable.SetValue(_gameTable.BikePos, _gameTable.Size.Y - 1, 1);
            }
            OnRefreshTable();
        }

        public void LeftMove()
        {
            if (_gameTable.BikePos - 1 >= 0)
            {
                _gameTable.SetValue(_gameTable.BikePos, _gameTable.Size.Y - 1, 0);
                _gameTable.BikePos--;
                _gameTable.SetValue(_gameTable.BikePos, _gameTable.Size.Y - 1, 1);
            }
            OnRefreshTable();
        }

        #endregion
    }
}
