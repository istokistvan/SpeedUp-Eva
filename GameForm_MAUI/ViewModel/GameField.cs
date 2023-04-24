namespace GameForm_MAUI.ViewModel
{
    public class GameField : ViewModelBase
    {

        private int _color;

        public int Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Number { get; set; }

    }
}
