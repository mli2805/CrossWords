using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<WordsDictVm> WordDictionaries { get; set; }
        public string DictionaryTotal { get; set; }
        public string DictionaryRemark { get; set; }
        public List<string>? SelectedWords { get; set; }

        private int _selectedCount;
        public int SelectedCount    
        {
            get => _selectedCount;
            set
            {
                if (value == _selectedCount) return;
                _selectedCount = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(SelectedCountMessage));
            }
        }

        public string SelectedCountMessage => $"Выбрано {SelectedCount} слов";

        private DataRepository _repository;

        public ShellViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;

            _repository = new DataRepository();

            WordDictionaries = new ObservableCollection<WordsDictVm>(_repository.GetWordSources().Result);
            foreach (var wordDictionary in WordDictionaries)
            {
                wordDictionary.PropertyChanged += WordDictionary_PropertyChanged;
            }

            DictionaryTotal = $"Словари содержат {_repository.GetCountAsync().Result} слов";
            DictionaryRemark = "Слова могут повторяться (Ожегов - Ефремова)";
        }

        private async void WordDictionary_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SelectedWords = await _repository.GetUniqueWordsAsync(WordDictionaries);
            SelectedCount = SelectedWords.Count;
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
            var result = await _repository.AddFromFile(fileToAdd, "peaks", "highest mountain peaks", TextToDbParsing.GeographyToDbWord);
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
            //_corpus = new Corpus().LoadFromTxt(CorpusFilename);
            if (SelectedWords == null) return;
            _corpus = new Corpus().FromList(SelectedWords);

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
