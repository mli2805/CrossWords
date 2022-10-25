using Caliburn.Micro;

namespace CrossWord
{
    public class WordsDictVm : PropertyChangedBase
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                NotifyOfPropertyChange();
            }
        }

        public string? Code { get; set; }
        public string? Description { get; set; }
        public int Count { get; set; }
        public string Message => $"содержит {Count} слов";
    }
}
