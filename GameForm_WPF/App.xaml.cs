using GameForm_WPF.View;
using GameForm_WPF.ViewModel;
using Microsoft.Win32;
using SpeedUp.Model;
using SpeedUp.Persistence;
using System;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Threading;

namespace GameForm_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private MainViewModel _mainViewModel;
        private GameModel _gameModel;
        private MainWindow _mainWindow;
        private DispatcherTimer _timer;
        private int _diffTime = 1000;

        #endregion

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object? caller, object _)
        {
            _gameModel = new GameModel(new DataAccess());
            _gameModel.GameOver += new EventHandler(ViewModel_GameOver);
            _gameModel.NewGame();

            _mainViewModel = new MainViewModel(_gameModel);
            _mainViewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _mainViewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            _mainViewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _mainViewModel.Pause += new EventHandler(ViewModel_Pause);

            _mainWindow = new MainWindow();
            _mainWindow.DataContext = _mainViewModel;
            _mainWindow.Show();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            int rand = r.Next(1, 100);
            _gameModel.TimerCount += (int)_timer.Interval.TotalMilliseconds;
            
            if (_gameModel.TimerCount % rand == 0 && _diffTime - 50 > 0)
            {
                _diffTime -= 50;
                _timer.Interval = TimeSpan.FromMilliseconds(_diffTime - 50);
            }

            if (_gameModel.GameTable.Gas > 0)
            {
                _gameModel.GameTable.Gas -= 2;
            }

            _gameModel.ProgressTime();
        }

        #region ViewModel EventHandlers

        private void ViewModel_NewGame(object sender, EventArgs e)
        {
            _diffTime = 1000;
            _mainViewModel.Model_NewGame();
            _timer.Start();
        }

        private async void ViewModel_SaveGame(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json files (*.json)|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    await _gameModel.SaveGame(saveFileDialog.OpenFile());
                } catch (Exception ex)
                {
                    MessageBox.Show("Sikertelen mentés!","SpeedUp!" + Environment.NewLine + "Wrong Path", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void ViewModel_LoadGame(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Json files (*.json)|*.json";
                if (openFileDialog.ShowDialog() == true)
                {
                    await _gameModel.LoadGame(openFileDialog.OpenFile());
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Sikertelen betöltés!", "SpeedUp!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewModel_Pause(object sender, EventArgs e)
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            } else
            {
                _timer.Start();
            }
        }

        private void ViewModel_GameOver(object sender, EventArgs e)
        {
            _timer.Stop();
            if (MessageBox.Show("Játék vége!" + Environment.NewLine + _gameModel.LifeTime + " másodpercig tartott a játék!" + Environment.NewLine + "Szeretne új játékot kezdeni?", "SpeedUp!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _gameModel.NewGame();
                _timer.Start();
                _timer.Interval = TimeSpan.FromSeconds(1);
                _gameModel.TimerCount = 0;
            } else
            {
                Application.Current.Shutdown();
            }
        }

        #endregion
    }
}
