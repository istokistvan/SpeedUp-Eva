using GameForm.Properties;
using SpeedUp.Model;
using SpeedUp.Persistence;
using Timer = System.Windows.Forms.Timer;

namespace GameForm
{
    public partial class GameForm : Form
    {

        #region Fields

        private GameModel _gameModel = null!;
        private Button[,] _buttonGrid = null!;
        private Timer _timer;
        private bool _paused = false;
        private IDataAccess? _dataAccess;

        #endregion

        public GameForm()
        {

            InitializeComponent();
            LoadNewGame();
            
        }

        #region Table

        private void Generate()
        {
            _buttonGrid = new Button[_gameModel.GameTable.Size.X, _gameModel.GameTable.Size.Y];

            for (int y = 0; y < _gameModel.GameTable.Size.Y; y++)
            {
                for (int x = 0; x < _gameModel.GameTable.Size.X; x++)
                {
                    _buttonGrid[x, y] = new Button();
                    _buttonGrid[x, y].Size = new Size(50, 50);
                    _buttonGrid[x, y].Location = new Point(5 + 50 * x, 35 + 50 * y);
                    _buttonGrid[x, y].Enabled = false;
                    _buttonGrid[x, y].FlatStyle = FlatStyle.Flat;

                    Controls.Add(_buttonGrid[x, y]);
                }
            }
            TableRefresh();
        }

        private void TableRefresh()
        {
            for (int y = 0; y < _gameModel.GameTable.Size.Y; y++)
            {
                for (int x = 0; x < _gameModel.GameTable.Size.X; x++)
                {
                    switch (_gameModel.GameTable.GetValue(x, y))
                    {
                        case 0: _buttonGrid[x, y].BackColor = Color.White; break;
                        case 1: _buttonGrid[x, y].BackColor = Color.Black; break;
                        case 2: _buttonGrid[x, y].BackColor = Color.Red; break;
                        default: break;
                    }
                }
            }
        }


        #endregion

        #region Controls

        private void Control(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A: _gameModel.LeftMove(); break;
                case Keys.Left: _gameModel.LeftMove(); break;
                case Keys.D: _gameModel.RightMove(); break;
                case Keys.Right: _gameModel.RightMove(); break;
                case Keys.Space: if (_paused) { Resume(); } else { Pause(); } break;
                case Keys.Escape: if (_paused) { Resume(); } else { Pause(); } break;
                default: break;
            }
            TableRefresh();
        }

        #endregion

        #region Game Control

        private void Pause()
        {
            _timer.Stop();
            _paused = true;
        }

        private void Resume()
        {
            _timer.Start();
            _paused = false;
        }

        private void LoadNewGame()
        {

            _dataAccess = new DataAccess();

            _gameModel = new GameModel(_dataAccess);

            _gameModel.GameOver += new EventHandler(GameOver);

            _timer = new Timer();
            _timer.Interval = 800;
            _timer.Tick += new EventHandler(TimerTick);

            _gameModel.NewGame();
            Generate();

            _timer.Start();
        }

        private void TimerTick(object? sender, EventArgs e)
        {
            Random r = new Random();
            int rand = r.Next(1, 100);
            _gameModel.TimerCount += _timer.Interval;
            if (_gameModel.TimerCount % rand == 0 && _timer.Interval - 20 > 0)
            {
                _timer.Interval -= 20;
            }

            if (_gameModel.GameTable.Gas > 0)
            {
                _gameModel.GameTable.Gas -= 2;
            }

            _gameModel.ProgressTime();

            if ( _gameModel.GameTable.Gas > 100)
            {
                GasBar.Value = 100;
            }
            else if (_gameModel.GameTable.Gas < 0)
            {
                GasBar.Value = 0;
            }
            else
            {
                GasBar.Value = _gameModel.GameTable.Gas;
            }

            TableRefresh();
        }

        private void GameOver(object? sender, EventArgs e)
        {
            Pause();

            switch (MessageBox.Show($"Játék vége!\n{_gameModel.LifeTime} másodpercig volt életben.\nSzeretne új játékot kezdeni?", "SpeedUp!", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    _gameModel.NewGame();
                    _timer.Start();
                    _timer.Interval = 1000;
                    _gameModel.TimerCount = 0;
                    _gameModel.GameTable.Gas = 50;
                    break;
                default:
                    Close();
                    break;
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Pause();

            switch(MessageBox.Show("Biztos ki szeretne lépni?\nA nem mentett játékállás el fog veszni!","SpeedUp!", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    Close();
                    break;
                default:
                    Pause();
                    break;
            }
        }

        private void newGame_Click(object sender, EventArgs e)
        {
            Pause();

            switch(MessageBox.Show("Biztosan szeretne új játékot kezdeni?", "SpeedUp!", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    _gameModel.NewGame();
                    _timer.Start();
                    _timer.Interval = 1000;
                    _gameModel.TimerCount = 0;
                    _gameModel.GameTable.Gas = 50;
                    break;
                default:
                    Pause();
                    break;

            }
        }

        private async void saveGame_Click(object sender, EventArgs e)
        {
            Pause();

            using(SaveFileDialog saveFileDialog = new SaveFileDialog())
            {

                saveFileDialog.Filter = "Json files (*.json)|*.json";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using var file = saveFileDialog.OpenFile();
                        await _gameModel.SaveGame(file);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Sikertelen mentés!\nHiba: {ex.Message}", "SpeedUp!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void loadGame_Click(object sender, EventArgs e)
        {
            Pause();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Json files (*.json)|*.json";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using var file = openFileDialog.OpenFile();
                        await _gameModel.LoadGame(file);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Sikertelen betöltés!\nHiba: {ex.Message}", "SpeedUp!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

            }
            
            TableRefresh();
        }

        #endregion


    }
}