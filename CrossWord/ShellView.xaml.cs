using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CrossWord
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView
    {
        private const int RectSize = 32;
        private const int HorShift = 12;
        private const int VertShift = 10;
        private const int Space = 2;
        private const int BoardLeft = 340;
        private const int BoardTop = 40;



        public ShellView()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(FileNameTextBlock.Text))
                ShowCrossBoard(FileNameTextBlock.Text);
        }

        private void ShowCrossBoard(string filename)
        {
            this.LeftPanel.Children.Clear();

            var crossBoard = new CrossBoard().LoadFromCsv(filename, ";");
            string[] rows = crossBoard.Rows;
            var places = crossBoard.GetPlaces();

            var canvas = new Canvas();
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows[i].Length; j++)
                {
                    if (rows[i][j] != '1')
                    {
                        DrawRect(canvas, i, j);
                        if (rows[i][j] != '0')
                            DrawLetter(canvas, rows[i][j], i, j);
                    }
                }
            }

            foreach (var place in places)
            {
                DrawNumber(canvas, place.PlaceNumber, place.P.StartIdx, place.LineNumber);
            }

            this.LeftPanel.Children.Add(canvas);
        }

        private static void DrawNumber(Canvas canvas, int number, int i, int j)
        {
            var t1 = new TextBlock();
            t1.Text = number.ToString();
            t1.FontSize = 12;
            canvas.Children.Add(t1);
            Canvas.SetLeft(t1, BoardLeft + i * (RectSize + Space) + 1);
            Canvas.SetTop(t1, BoardTop + j * (RectSize + Space) + 1);
        }

        private static void DrawLetter(Canvas canvas, char letter, int i, int j)
        {
            var t = new TextBlock();
            t.Text = letter.ToString();
            t.FontSize = 13;
            canvas.Children.Add(t);
            Canvas.SetLeft(t, BoardLeft + i * (RectSize + Space) + HorShift);
            Canvas.SetTop(t, BoardTop + j * (RectSize + Space) + VertShift);
        }

        private void DrawRect(Canvas canvas, int i, int j)
        {
            var rect = CreateRect(RectSize);
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, BoardLeft + i * (RectSize + Space));
            Canvas.SetTop(rect, BoardTop + j * (RectSize + Space));
        }

        private Rectangle CreateRect(int size)
        {
            return new Rectangle
            {
                Width = size,
                Height = size,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
            };
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FileNameTextBlock.Text))
                ShowCrossBoard(FileNameTextBlock.Text);
        }
    }
}
