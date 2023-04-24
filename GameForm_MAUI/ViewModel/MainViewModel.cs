using SpeedUp.Model;
using System.Collections.ObjectModel;

namespace GameForm_MAUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private GameModel _gameModel = null!;
        private int gasValue;

        #endregion

        #region Properties

        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand SaveGameCommand { get; private set; }

        public DelegateCommand LoadGameCommand { get; private set; }

        public DelegateCommand PauseCommand { get; private set; }

        public DelegateCommand MoveLeftCommand { get; private set; }

        public DelegateCommand MoveRightCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public int GasValue { get => gasValue; private set { gasValue = value; OnPropertyChanged(); } }

        public ObservableCollection<GameField> Fields { get; set; }

        #endregion

        #region Events

        public event EventHandler Pause;

        public event EventHandler NewGame;

        public event EventHandler LoadGame;

        public event EventHandler SaveGame;

        public event EventHandler Exit;

        #endregion

        #region Constructor

        public MainViewModel(GameModel model)
        {
            _gameModel = model;
            _gameModel.RefreshTable += new EventHandler(Model_GameStep);
            _gameModel.GasValueChange += OnGasChange;

            NewGameCommand = new DelegateCommand(param => OnNewGameCommand());
            SaveGameCommand = new DelegateCommand(param => OnSaveGameCommand());
            LoadGameCommand = new DelegateCommand(param => OnLoadGameCommand());
            PauseCommand = new DelegateCommand(param => OnPauseCommand());
            MoveLeftCommand = new DelegateCommand(param => OnMoveLeftCommand());
            MoveRightCommand = new DelegateCommand(param => OnMoveRightCommand());
            ExitCommand = new DelegateCommand(param => OnExitGameCommand());

            Fields = new ObservableCollection<GameField>();
            for (int y = 0; y < _gameModel.GameTable.Size.Y; y++)
            {
                for (int x = 0; x < _gameModel.GameTable.Size.X; x++)
                {
                    Fields.Add(new GameField
                    {
                        Color = _gameModel.GameTable.GetValue(x, y),
                        X = x,
                        Y = y,
                        Number = x * _gameModel.GameTable.Size.X + y,
                    }); ;
                }
            }

            TableRefresh();
        }

        #endregion

        #region Eventhandlers

        private void Model_GameStep(object? caller, object _)
        {
            TableRefresh();
        }

        public void Model_NewGame()
        {
            _gameModel.NewGame();
            TableRefresh();
        }

        #endregion

        #region Event Methods

        private void OnGasChange(object? caller, int value)
        {
            GasValue = value;
        }

        private void OnNewGameCommand()
        {
            if (NewGame != null)
                NewGame(this, EventArgs.Empty);
        }

        private void OnSaveGameCommand()
        {
            OnPauseCommand();
            if (SaveGame != null)
                SaveGame(this, EventArgs.Empty);
        }

        private void OnLoadGameCommand()
        {
            OnPauseCommand();
            if (LoadGame != null)
                LoadGame(this, EventArgs.Empty);
        }

        private void OnPauseCommand()
        {
            if (Pause != null)
                Pause(this, EventArgs.Empty);
        }

        private void OnMoveLeftCommand()
        {
            _gameModel.LeftMove();
        }

        private void OnMoveRightCommand()
        {
            _gameModel.RightMove();
        }

        private void OnExitGameCommand()
        {
            Exit?.Invoke(this, EventArgs.Empty);
        }

        public RowDefinitionCollection GameTableRows
        {
            get => new RowDefinitionCollection(Enumerable.Repeat(new RowDefinition(GridLength.Star), _gameModel.GameTable.Size.Y).ToArray());
        }

        public ColumnDefinitionCollection GameTableColumns
        {
            get => new ColumnDefinitionCollection(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), _gameModel.GameTable.Size.X).ToArray());
        }

        #endregion

        #region Private Methods

        private void TableRefresh()
        {
            foreach (GameField field in Fields)
                field.Color = _gameModel.GameTable.GetValue(field.X, field.Y);
        }

        #endregion

    }
}
