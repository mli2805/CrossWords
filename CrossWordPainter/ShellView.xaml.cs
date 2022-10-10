using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CrossWordPainter
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView
    {
        public ShellView()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(FileNameTextBlock.Text))
                ShowCrossBoard(FileNameTextBlock.Text);
        }

        private void ShowCrossBoard(string filename)
        {
            char[][] arr = LoadFromCsv(filename, ";");

            var canvas = new Canvas();
            var rectSize = 32;
            var horShift = 12;
            var vertShift = 10;
            var space = 2;
            var left = 340;
            var top = 40;

            for (int i = 0; i < arr.Length; i++)
            {
                var line = arr[i];
                for (int j = 0; j < line.Length; j++)
                {
                    if (line[j] != '1')
                    {
                        var rect = CreateRect(rectSize);
                        canvas.Children.Add(rect);
                        Canvas.SetLeft(rect, left + i * (rectSize + space));
                        Canvas.SetTop(rect, top + j * (rectSize + space));
                        if (line[j] != '0')
                        {
                            var t = new TextBlock();
                            t.Text = line[j].ToString();
                            t.FontSize = 13;
                            canvas.Children.Add(t);
                            Canvas.SetLeft(t, left + i * (rectSize + space) + horShift);
                            Canvas.SetTop(t, top + j * (rectSize + space) + vertShift);
                        }
                    }

                }

            }

            var t1 = new TextBlock();
            t1.Text = "14";
            t1.FontSize = 9;
            canvas.Children.Add(t1);
            Canvas.SetLeft(t1, left + 1 * (rectSize + space) + 1);
            Canvas.SetTop(t1, top + 1 * (rectSize + space) + 1);

            this.LeftPanel.Children.Add(canvas);

            // this.Content = canvas;
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


        public char[][] LoadFromCsv(string filename, string csvSeparator)
        {
            var content = File.ReadAllLines(filename);
            var result = new char[content.Length][];
            for (int i = 0; i < content.Length; i++)
            {
                var symbols = content[i].Replace(csvSeparator, "");
                result[i] = new char[symbols.Length];
                for (int j = 0; j < symbols.Length; j++)
                {
                    result[i][j] = symbols[j];
                }
            }
            return result;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FileNameTextBlock.Text))
                ShowCrossBoard(FileNameTextBlock.Text);
        }
    }
}
