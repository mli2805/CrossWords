using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Win32;

namespace CrossWord
{
    /// <summary>
    /// Interaction logic for PainterView.xaml
    /// </summary>
    public partial class PainterView
    {
        private const int RectSize = 32;
        private const int Space = 2;
        private const int PlacesInQuarter = 20;

        private bool _isHorizontalAxisThroughLetter;
        private bool _isVerticalAxisThroughLetter;
       
        private double QuarterHorizontalSize => (RectSize + Space) * (PlacesInQuarter - (_isVerticalAxisThroughLetter ? 0.5 : 0));
        private double QuarterVerticalSize => (RectSize + Space) * (PlacesInQuarter - (_isHorizontalAxisThroughLetter ? 0.5 : 0));

        public PainterView()
        {
            InitializeComponent();

            SetCanvasesSize();
            DrawRectangles();
        }

        private bool[,] _quarter = new bool[PlacesInQuarter, PlacesInQuarter];

        /// <summary>
        ///  1 |  2
        /// --------
        ///  3 |  4
        /// </summary>
        private void DrawRectangles()
        {
            MyCanvas1.DrawQuarter(_quarter, RectSize, Space, false, false, false);
            var quarter2 = _quarter.Turn(true);
            MyCanvas2.DrawQuarter(quarter2, RectSize, Space, _isVerticalAxisThroughLetter, false, true);
            MyCanvas3.DrawQuarter(_quarter.Turn(false), RectSize, Space,
                false, _isHorizontalAxisThroughLetter, true);
            MyCanvas4.DrawQuarter(quarter2.Turn(false), RectSize, Space,
                _isVerticalAxisThroughLetter, _isHorizontalAxisThroughLetter, true);
        }

        private void MyCanvas1_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var point = e.GetPosition((Canvas)sender);
            Debug.WriteLine(point);
            var row = (int)point.X / (RectSize + Space);
            var column = (int)point.Y / (RectSize + Space);
            _quarter[row, column] = !_quarter[row, column];

            DrawRectangles();
        }

        private void SaveAs(object sender, System.Windows.RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "csv files (*.csv)|*.csv";
            if (dlg.ShowDialog() == true)
            {
                File.WriteAllLines(dlg.FileName,
                    _quarter
                        .ToFullBoard(_isHorizontalAxisThroughLetter, _isVerticalAxisThroughLetter)
                        .ToStrings()
                        .Trim()
                        .Wrap()
                        .Select(row => string.Join(';', row.ToCharArray())));
            }
        }

        private const string Path = "C:\\VsGitProjects\\CrossWords\\Data\\";

        private void Open(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Path;
            dlg.Filter = "csv files (*.csv)|*.csv";
            if (dlg.ShowDialog() == true)
            {
                OpenFileName.Text = dlg.FileName;
            }

            var content = File.ReadAllLines(OpenFileName.Text);
            _isHorizontalAxisThroughLetter = content.Length % 2 == 1;
            _isVerticalAxisThroughLetter = (content[0].Length + 1) / 2 % 2 == 1;
            CbHorizontal.IsChecked = _isHorizontalAxisThroughLetter;
            CbVertical.IsChecked = _isVerticalAxisThroughLetter;
            _quarter = content
                .GetQuarterWithoutSeparator(';').ToArray()
                .ToBool(PlacesInQuarter);

            SetCanvasesSize();
            ClearCanvases();
            DrawRectangles();
        }

        private void CbHorizontal_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CbHorizontal.IsChecked == null) return;

            _isHorizontalAxisThroughLetter = (bool)CbHorizontal.IsChecked;
            SetCanvasesSize();
            ClearCanvases();
            DrawRectangles();
        }

        private void CbVertical_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CbVertical.IsChecked == null) return;

            _isVerticalAxisThroughLetter = (bool)CbVertical.IsChecked;
            SetCanvasesSize();
            ClearCanvases();
            DrawRectangles();
        }

        private void ClearCanvases()
        {
            MyCanvas1.Children.Clear();
            MyCanvas2.Children.Clear();
            MyCanvas3.Children.Clear();
            MyCanvas4.Children.Clear();
        }

        private void SetCanvasesSize()
        {
            MyCanvas1.Height = QuarterVerticalSize;
            MyCanvas1.Width = QuarterHorizontalSize;
            MyCanvas2.Height = QuarterVerticalSize;
            MyCanvas2.Width = QuarterHorizontalSize;
            MyCanvas3.Width = QuarterHorizontalSize;
            MyCanvas4.Width = QuarterHorizontalSize;
        }
    }
}
