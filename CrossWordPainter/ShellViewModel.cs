using Caliburn.Micro;
using Microsoft.Win32;

namespace CrossWordPainter
{
    public class ShellViewModel : Screen, IShell
    {
        private string _selectedFile = "";

        public string SelectedFile
        {
            get => _selectedFile;
            set
            {
                if (value == _selectedFile) return;
                _selectedFile = value;
                NotifyOfPropertyChange();
            }
        }

        public void SelectFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "C:\\VsGitProjects\\CrossWords\\Data\\";
            dlg.Filter = "csv files (*.csv)|*.csv";
            if (dlg.ShowDialog() == true)
            {
                SelectedFile = dlg.FileName;
            }
        }
    }
}
