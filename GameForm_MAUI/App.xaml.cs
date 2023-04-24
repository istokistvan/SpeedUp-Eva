using GameForm_MAUI.Persistence;
using GameForm_MAUI.ViewModel;
using SpeedUp.Model;
using SpeedUp.Persistence;

namespace GameForm_MAUI
{
    public partial class App : Application
    {

        private const string SuspendedGameSavePath = "SuspendedGame";

        private readonly AppShell _appShell;
        private readonly IDataAccess _dataAccess;
        private readonly GameModel _gameModel;
        private readonly IStore _store;
        private readonly MainViewModel _mainViewModel;

        public App()
        {
            InitializeComponent();

            _store = new Store();
            _dataAccess = new DataAccess();
            _gameModel = new GameModel(_dataAccess);
            _mainViewModel = new MainViewModel(_gameModel);

            _appShell = new AppShell(_store, _dataAccess, _gameModel, _mainViewModel)
            {
                BindingContext = _mainViewModel
            };

            MainPage = _appShell;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);
            window.Created += (s, e) =>
            {
                _gameModel.NewGame();
                _appShell.Start();
            };

            window.Activated += (s, e) =>
            {
                if (!File.Exists(Path.Combine(FileSystem.AppDataDirectory, SuspendedGameSavePath))) return;
                Task.Run(async () =>
                {
                    try
                    {
                        using (var stream = File.OpenRead(Path.Combine(FileSystem.AppDataDirectory, SuspendedGameSavePath)))
                        {
                            await _gameModel.LoadGame(stream);
                        }
                        _appShell.Start();
                    } catch
                    {

                    }
                });
            };

            window.Stopped += (s, e) =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        using (var stream = File.OpenWrite(Path.Combine(FileSystem.AppDataDirectory, SuspendedGameSavePath)))
                        {
                            _appShell.Stop();
                            await _gameModel.SaveGame(stream);
                        }
                    }
                    catch { }
                });
            };

            return window;
        }
    }
}