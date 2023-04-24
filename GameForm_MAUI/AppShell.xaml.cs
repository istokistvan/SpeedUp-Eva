using SpeedUp.Model;
using SpeedUp.Persistence;
using GameForm_MAUI.ViewModel;
using GameForm_MAUI.View;
using Microsoft.Maui.ApplicationModel;
using System.Text;

namespace GameForm_MAUI
{
    public partial class AppShell : Shell
    {

        private IDataAccess _dataAccess;
        private GameModel _gameModel;
        private MainViewModel _viewModel;

        private IDispatcherTimer _timer;

        private IStore _store;
        private StoredGameBrowserModel _storedGameBrowserModel;
        private StoredGameBrowserViewModel _storedGameBrowserViewModel;
        private bool _paused = false;


        public AppShell(IStore store,
                        IDataAccess dataAccess,
                        GameModel gameModel,
                        MainViewModel viewModel)
        {
            InitializeComponent();

            _store = store;
            _dataAccess = dataAccess;
            _gameModel = gameModel;
            _viewModel = viewModel;

            _timer = Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (_, _) => _gameModel.ProgressTime();

            _gameModel.GameOver += GameModel_GameOver;
            _viewModel.NewGame += ViewModel_NewGame;
            _viewModel.LoadGame += ViewModel_Load;
            _viewModel.SaveGame += ViewModel_Save;
            _viewModel.Exit += ViewModel_Exit;
            _viewModel.Pause += ViewModel_Pause;

            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
            _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
        }

        internal void Start() {
            _timer.Start();
            _paused = false;
        }

        internal void Stop() { 
            _timer.Stop(); 
            _paused= true;
        }

        private async void GameModel_GameOver(object? sender, EventArgs e)
        {
            Stop();

                await DisplayAlert("SpeedUp!",
                    "Sajnos vége a játéknak." + Environment.NewLine +
                    TimeSpan.FromSeconds(_gameModel.LifeTime).ToString() + "ideig játszottál!",
                    "OK");
        }

        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            _gameModel.NewGame();
            Start();
        }

        private async void ViewModel_Load(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync();
            await Navigation.PushAsync(new LoadGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            });
        }

        private async void ViewModel_Save(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync();
            await Navigation.PushAsync(new SaveGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            });
        }

        private async void ViewModel_Exit(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage
            {
                BindingContext = _viewModel
            });
        }

        private void ViewModel_Pause(object? sender, EventArgs e)
        {
            if (_paused)
            {
                Start();
            } else
            {
                Stop();
            }
        }

        private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync();

            try
            {
                using (var stream = File.OpenRead(Path.Join(FileSystem.Current.AppDataDirectory, e.Name))) {
                    await _gameModel.LoadGame(stream);
                }

                await Navigation.PopAsync();
                await DisplayAlert("SpeedUp!", "Sikeres betöltés!", "OK");

                Start();
            } catch
            {
                await DisplayAlert("SpeedUp!", "Sikertelen betöltés!", "OK");
            }
        }

        private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync();
            Stop();

            try
            {
                using (var stream = File.OpenWrite(Path.Join(FileSystem.Current.AppDataDirectory,e.Name)))
                {
                    await _gameModel.SaveGame(stream);
                }
                await DisplayAlert("SpeedUp!", "Sikeres mentés!", "OK");
            } catch
            {
                await DisplayAlert("SpeedUp!", "Sikertelen mentés!", "OK");
            }
        }
    }
}