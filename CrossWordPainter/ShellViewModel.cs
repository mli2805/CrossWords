using System.ComponentModel;
using Caliburn.Micro;
using CrossWordFiller;
using Microsoft.Win32;

namespace CrossWordPainter
{
    public class ShellViewModel : Screen, IShell
    {
        private const string Path = "C:\\VsGitProjects\\CrossWords\\Data\\";
        // private const string CorpusFilename = Path + "words.txt";
        private const string JsonFile = "c:\\VsGitProjects\\CrossWords\\Dictionaries\\harrix.dev\\russian_nouns_with_definition.json";

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

        private string _message = "";
        public string Message
        {
            get => _message;
            set
            {
                if (value == _message) return;
                _message = value;
                NotifyOfPropertyChange();
            }
        }

        public void SelectFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Path;
            dlg.Filter = "csv files (*.csv)|*.csv";
            if (dlg.ShowDialog() == true)
            {
                SelectedFile = dlg.FileName;
            }
        }

        private CrossBoard _board = new();
        private Corpus? _corpus;
        private BackgroundWorker? _bw;
        public void Compose()
        {
            Message = "";
            _board = new CrossBoard().LoadFromCsv(SelectedFile, ";");
            if (_board == null) return;
            _corpus = new Corpus().LoadHarrixEfremovaJson(JsonFile);

            _bw = new BackgroundWorker();
            _bw.WorkerReportsProgress = true;
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += Bw_DoWork;
            _bw.ProgressChanged += Bw_ProgressChanged;
            _bw.RunWorkerCompleted += Bw_RunWorkerCompleted;

            _bw.RunWorkerAsync();
        }

        public void Interrupt()
        {
            _bw?.CancelAsync();
        }

        private void Bw_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            Message = $"{e.ProgressPercentage}%";
        }

        private void Bw_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // handle the error
                Message = "Ошибка!";
            }
            else if (e.Cancelled)
            {
                // handle cancellation
                Message = "Прервано!";
            }
            else
            {
                if (e.Result == null) return;
                ComposerResult r = (ComposerResult)e.Result;
                // use it on the UI thread
                if (!r.IsFailed)
                {
                    var newFilename = Path + "result.csv";
                    _board.SaveToCsv(newFilename, ";");
                    SelectedFile = newFilename;
                    Message = "Готово!";
                }
                else Message = "Не удалось составить!"; }
        }

        private void Bw_DoWork(object? sender, DoWorkEventArgs e)
        {
            var r = _board.Fill(_corpus, _bw);
            if (r.IsCanceled)
                e.Cancel = true;
            e.Result = r;
        }
    }
}
