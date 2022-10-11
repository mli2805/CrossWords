using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
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
        private const double QuarterSize = (RectSize + Space) * 20;

        public PainterView()
        {
            InitializeComponent();
            MyCanvas1.Height = QuarterSize;
            MyCanvas1.Width = QuarterSize;
            MyCanvas2.Width = QuarterSize;

            DrawRectangles();
        }

        private bool[,] _quarter = new bool[20, 20];

        public void DrawRectangles()
        {
            DrawQuarter(MyCanvas1, _quarter, false);
            var quarter2 = Turn(_quarter, true);
            DrawQuarter(MyCanvas2, quarter2, true);
            DrawQuarter(MyCanvas3, Turn(quarter2, false), true);
            DrawQuarter(MyCanvas4, Turn(_quarter, false), true);
        }

        private void DrawQuarter(Canvas canvas, bool[,] board, bool isShadowed)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    Rectangle rectangle = new Rectangle
                    {
                        Height = RectSize,
                        Width = RectSize,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                    };

                    rectangle.Fill = isShadowed
                        ? board[i, j] ? Brushes.LightGray : Brushes.DarkGray
                        : board[i, j] ? Brushes.White : Brushes.LightGray;
                    canvas.Children.Add(rectangle);

                    Canvas.SetLeft(rectangle, i * (RectSize + Space));
                    Canvas.SetTop(rectangle, j * (RectSize + Space));
                }
            }
        }

        private bool[,] Turn(bool[,] quarter, bool horizontal)
        {
            var result = new bool[quarter.GetLength(0), quarter.GetLength(1)];

            for (int j = 0; j < quarter.GetLength(1); j++)
            {
                for (int i = 0; i < quarter.GetLength(0); i++)
                {
                    if (horizontal)
                        result[quarter.GetLength(0) - i - 1, j] = quarter[i, j];
                    else
                        result[quarter.GetLength(0) - i - 1, quarter.GetLength(1) - j - 1] = quarter[i, j];
                }
            }

            return result;
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
    }
}
