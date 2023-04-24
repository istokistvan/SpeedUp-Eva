using Moq;
using SpeedUp.Model;
using SpeedUp.Persistence;

namespace SpeedUpTest
{
    [TestClass]
    public class GameTest
    {
        private Mock<IDataAccess> _mock = null!;
        private GameModel _model = null!;

        [TestInitialize]
        public void Initialize()
        {
            _mock = new Mock<IDataAccess>();
            _model = new GameModel(_mock.Object);
        }

        [TestMethod]
        public void NewGameTest()
        {
            _model.NewGame();

            Assert.AreEqual(0, _model.TimerCount);

            int emptyFields = 0;
            for (int y = 0; y < _model.GameTable.Size.Y; y++)
            {
                for (int x = 0; x < _model.GameTable.Size.X; x++)
                {
                    if (_model.GameTable.GetValue(x, y) == 0)
                    {
                        emptyFields++;
                    }
                }
            }
            Assert.AreEqual((_model.GameTable.Size.Y * _model.GameTable.Size.X - 1) - 1, emptyFields);
        }

        [TestMethod]
        public void MoveTest()
        {
            _model.NewGame();
            
            _model.RightMove();
            Assert.AreEqual(_model.GameTable.GetValue(3, 9), 1);

            _model.LeftMove();
            Assert.AreEqual(_model.GameTable.GetValue(2, 9), 1);

            for(int i=0; i<7; i++)
            {
                _model.RightMove();
            }
            Assert.AreEqual(_model.GameTable.GetValue(4, 9), 1);
            
            for(int i=0; i<7; i++)
            {
                _model.LeftMove();
            }
            Assert.AreEqual(_model.GameTable.GetValue(0, 9), 1);
        }

        [TestMethod]
        public void StepTest()
        {
            _model.NewGame();

            _model.GameTable.SetValue(0, 0, 2);
            _model.GameTable.StepValue(0,0);
            Assert.AreEqual(_model.GameTable.GetValue(0, 1), 2);

        }
    }
}