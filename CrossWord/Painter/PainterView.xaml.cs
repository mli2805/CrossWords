using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using CrossWord.Annotations;
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
        private const int LettersInQuarter = 20;

        public bool IsHorizontalAxisThroughLetter { get; set; }
        public bool IsVerticalAxisThroughLetter { get; set; }
       
        private double QuarterHorizontalSize => (RectSize + Space) * (LettersInQuarter - (IsVerticalAxisThroughLetter ? 0.5 : 0));
        private double QuarterVerticalSize => (RectSize + Space) * (LettersInQuarter - (IsHorizontalAxisThroughLetter ? 0.5 : 0));

        public PainterView()
        {
            InitializeComponent();
            MyCanvas1.Height = QuarterVerticalSize;
            MyCanvas1.Width = QuarterHorizontalSize;
            MyCanvas2.Height = QuarterVerticalSize;
            MyCanvas2.Width = QuarterHorizontalSize;
            MyCanvas3.Width = QuarterHorizontalSize;
            MyCanvas4.Width = QuarterHorizontalSize;

            DrawRectangles();
        }

        private bool[,] _quarter = new bool[LettersInQuarter, LettersInQuarter];

        /// <summary>
        ///  1 |  2
        /// --------
        ///  3 |  4
        /// </summary>
        private void DrawRectangles()
        {
            MyCanvas1.DrawQuarter(_quarter, RectSize, Space, false, false, false);
            var quarter2 = _quarter.Turn(true);
            MyCanvas2.DrawQuarter(quarter2, RectSize, Space, IsVerticalAxisThroughLetter, false, true);
            MyCanvas3.DrawQuarter(_quarter.Turn(false), RectSize, Space,
                false, IsHorizontalAxisThroughLetter, true);
            MyCanvas4.DrawQuarter(quarter2.Turn(false), RectSize, Space,
                IsVerticalAxisThroughLetter, IsHorizontalAxisThroughLetter, true);
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
            if (dlg.ShowDialog() == true)
            {
                File.WriteAllLines(dlg.FileName,
                    _quarter
                        .ToFullBoard()
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

        }

        private void CbHorizontal_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CbHorizontal.IsChecked == null) return;

            IsHorizontalAxisThroughLetter = (bool)CbHorizontal.IsChecked;
            MyCanvas1.Height = QuarterVerticalSize;
            MyCanvas1.Width = QuarterHorizontalSize;
            MyCanvas2.Height = QuarterVerticalSize;
            MyCanvas2.Width = QuarterHorizontalSize;   
            MyCanvas3.Width = QuarterHorizontalSize;
            MyCanvas4.Width = QuarterHorizontalSize; 
            MyCanvas2.Children.Clear();
            MyCanvas3.Children.Clear();
            MyCanvas4.Children.Clear();
            DrawRectangles();
        }

        private void CbVertical_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CbVertical.IsChecked == null) return;

            IsVerticalAxisThroughLetter = (bool)CbVertical.IsChecked;
            MyCanvas1.Height = QuarterVerticalSize;
            MyCanvas1.Width = QuarterHorizontalSize;
            MyCanvas2.Height = QuarterVerticalSize;
            MyCanvas2.Width = QuarterHorizontalSize;
            MyCanvas3.Width = QuarterHorizontalSize;
            MyCanvas4.Width = QuarterHorizontalSize;
            MyCanvas2.Children.Clear();
            MyCanvas3.Children.Clear();
            MyCanvas4.Children.Clear();
            DrawRectangles();
        }
    }
}
