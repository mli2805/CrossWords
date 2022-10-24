using System.ComponentModel;
using System.Diagnostics;
using Caliburn.Micro;
using Microsoft.Win32;

namespace CrossWord
{
    public class ShellViewModel : Screen, IShell
    {
        private readonly IWindowManager _windowManager;
        private const string Path = "C:\\VsGitProjects\\CrossWords\\Data\\";
        private const string DictPath = "C:\\VsGitProjects\\CrossWords\\Dictionaries\\";
        private const string CorpusFilename = Path + "words.txt";
        // private const string JsonFile = "c:\\VsGitProjects\\CrossWords\\Dictionaries\\harrix.dev\\russian_nouns_with_definition.json";

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

        public string DictionaryTotal { get; set; }
        public string DictionaryRemark { get; set; }

        private DataRepository _repository;

        public ShellViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;

            _repository = new DataRepository();

            DictionaryTotal = $"Dictionary contains {_repository.GetCountAsync().Result} words";
            DictionaryRemark = "Слова могут повторяться (Ожегов - Ефремова)";
        }

        public async void AddWordFromFileToDict()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = DictPath;
            dlg.Filter = "text files (*.txt)|*.txt";
            if (dlg.ShowDialog() != true) return;


            using var cursor = new WaitCursor();
            var fileToAdd = dlg.FileName;
            // var result = await _repository.AddFromFile(fileToAdd, "efremova", "Словарь Ефремовой", TextToDbParsing.EfremovaToDbWord);
            // var result =await _repository.AddFromFile(fileToAdd, "ozhegov", "Словарь С.И.Ожегова", TextToDbParsing.OzhegovToDbWord);
            var result =await _repository.AddFromFile(fileToAdd, "peaks", "highest mountain peaks", TextToDbParsing.GeographyToDbWord);
            Debug.WriteLine(result);
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

        private CrossBoard _board = new CrossBoard();
        private Corpus _corpus = new Corpus();
        private BackgroundWorker _bw = new BackgroundWorker();
        public void Compose()
        {
            Message = "";
            _board = new CrossBoard().LoadFromCsv(SelectedFile, ";");
            // _corpus = new Corpus().LoadHarrixEfremovaJson(JsonFile);
            _corpus = new Corpus().LoadFromTxt(CorpusFilename);

            _bw.WorkerReportsProgress = true;
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += Bw_DoWork;
            _bw.ProgressChanged += Bw_ProgressChanged;
            _bw.RunWorkerCompleted += Bw_RunWorkerCompleted;

            _bw.RunWorkerAsync();
        }

        public void Interrupt()
        {
            _bw.CancelAsync();
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
                else Message = "Не удалось составить!";
            }
        }

        private void Bw_DoWork(object? sender, DoWorkEventArgs e)
        {
            var r = _board.Fill(_corpus, _bw);
            if (r.IsCanceled)
                e.Cancel = true;
            e.Result = r;
        }

        public void DrawBoard()
        {
            var vm = new PainterViewModel();
            _windowManager.ShowDialogAsync(vm);
        }

        public void Close()
        {
            TryCloseAsync();
        }
    }
}
